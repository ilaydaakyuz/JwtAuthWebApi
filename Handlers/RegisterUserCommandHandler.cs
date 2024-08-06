using MediatR;
using Microsoft.AspNetCore.Identity;
using MyWebApi.Enums;
using MyWebApi.Models;

namespace MyWebApi.Handlers;
public class RegisterUserCommandHandler(UserManager<IdentityUser> _userManager, ILogger<RegisterUserCommandHandler> _logger) : IRequestHandler<RegisterUserCommand, Result>
{

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.ClientId ?? string.Empty);
        if (existingUser != null)
        {
            _logger.LogError("User registration failed: User already exists.");
            return new Result
            {
                isSuccess = false,
                error = new ErrorModel(
                    ErrorType.Conflict,
                    "USER_ALREADY_EXIST",
                    "Kullanıcı zaten mevcut",
                    $"E-posta adresi {request.ClientId} olan kullanıcı zaten mevcut"
                )
            };
        }

        var user = new IdentityUser { UserName = request.ClientId, Email = request.ClientId };
        var result = await _userManager.CreateAsync(user, request.ClientSecret ?? string.Empty);

        if (result.Succeeded)
        {
            return new Result
            {
                isSuccess = true,
                Message = "User registered successfully"
            };
        }

        _logger.LogError("User registration failed: {@Errors}", result.Errors);
        var errorDetails = result.Errors.Select(e => e.Description).ToList();
        return new Result
        {
            isSuccess = false,
            error = new ErrorModel(
                ErrorType.ValidationError,
                "VALIDATION_ERROR",
                "Kullanıcı kaydı doğrulama hatası nedeniyle başarısız oldu.",
                string.Join("; ", errorDetails)
            )
        };
    }
}
