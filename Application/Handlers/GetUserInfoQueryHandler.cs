using System.Text.Json;
using MediatR;
using MyWebApi.Domain.Commands;
using MyWebApi.Domain.Enums;
using MyWebApi.Domain.Models;
using StackExchange.Redis;

namespace MyWebApi.Application.Handlers;

public class GetUserInfoQueryHandler(IConnectionMultiplexer _connectionMultiplexer, ILogger<GetUserInfoQueryHandler> _logger) : IRequestHandler<GetUserInfoQuery, UserInfoResult>
{

    public async Task<UserInfoResult> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"GetUserInfo query handled for userId: {request.UserId}");

        try
        {
            var cacheKey = $"MyWebApiUserSession:{request.UserId}";
            _logger.LogInformation($"Cache key: {cacheKey}");

            var db = _connectionMultiplexer.GetDatabase();
            var hashEntries = await db.HashGetAllAsync(cacheKey);

            if (hashEntries.Length == 0)
            {
                _logger.LogWarning($"User info not found in cache for user ID: {request.UserId}");
                return new UserInfoResult
                {
                    Error = new ErrorModel(
                        ErrorType.NotFound,
                        "USER_NOT_FOUND",
                        "User information not found",
                        $"No cached information found for user ID: {request.UserId}"
                    )
                };
            }

            var userInfo = hashEntries.ToDictionary(
                he => he.Name.ToString(),
                he => he.Value.ToString()
            );

            _logger.LogInformation($"Retrieved user info: {JsonSerializer.Serialize(userInfo)}");

            return new UserInfoResult { UserInfo = userInfo };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving user info for user ID: {request.UserId}");
            return new UserInfoResult
            {
                Error = new ErrorModel(
                    ErrorType.InternalServerError,
                    "INTERNAL_SERVER_ERROR",
                    "An error occurred while retrieving user information",
                    ex.Message
                )
            };
        }
    }
}