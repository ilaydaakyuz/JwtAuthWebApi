using MediatR;
using Microsoft.AspNetCore.Identity;
using MyWebApi.Application.Services;
using MyWebApi.Domain.Commands;
using MyWebApi.Domain.Enums;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;
using StackExchange.Redis;

namespace MyWebApi.Application.Handlers;

public class LoginQueryHandler(UserManager<IdentityUser> _userManager,
SignInManager<IdentityUser> _signInManager,
IJwtService _jwtService,
IConnectionMultiplexer _connectionMultiplexer,
ILogger<LoginQueryHandler> _logger) : IRequestHandler<LoginQuery, LoginResult>
{

    public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.ClientId ?? string.Empty);
            if (user == null)
            {
                return new LoginResult
                {
                    Error = new ErrorModel(
                        errorType: ErrorType.Unauthorized,
                        errorCode: "INVALID CREDENTIALS",
                        errorMessage: "Geçersiz kimlik bilgileri",
                        errorDetail: "Verilen kimlik bilgileriyle eşleşen bir kullanıcı bulunamadı."
                    )
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.ClientSecret ?? string.Empty, false);

            if (result.Succeeded)
            {
                var token = _jwtService.GenerateToken(user.UserName!);
                var userInfo = new Dictionary<string, string>
                {
                    ["UserId"] = user.Id,
                    ["UserName"] = user.UserName ?? "Unknown",
                    ["Email"] = user.Email ?? "Unknown",
                    ["LoginTime"] = DateTime.UtcNow.ToString("o")
                };

                var cacheKey = $"MyWebApiUserSession:{user.Id}";

                if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                {
                    throw new InvalidOperationException("Redis connection is not available");
                }
                var db = _connectionMultiplexer.GetDatabase();

                await db.HashSetAsync(cacheKey, userInfo.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray());
                await db.KeyExpireAsync(cacheKey, TimeSpan.FromHours(1));

                return new LoginResult
                {
                    Token = token,
                    UserId = user.Id
                };
            }

            return new LoginResult
            {
                Error = new ErrorModel(
                    errorType: ErrorType.Unauthorized,
                    errorCode: "INVALID CREDENTIALS",
                    errorMessage: "Geçersiz kimlik bilgileri",
                    errorDetail: "Verilen şifre yanlış."
                )
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login");
            return new LoginResult
            {
                Error = new ErrorModel(
                    errorType: ErrorType.InternalServerError,
                    errorCode: "LOGIN_ERROR",
                    errorMessage: "Giriş sırasında bir hata oluştu",
                    errorDetail: "Lütfen daha sonra tekrar deneyin."
                )
            };
        }
    }
}