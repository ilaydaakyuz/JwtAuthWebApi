using MediatR;
using MyWebApi.Application.Services;
using MyWebApi.Domain.Commands;
using MyWebApi.Domain.Enums;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;

namespace MyWebApi.Application.Handlers;
public class ValidateTokenQueryHandler(ITokenValidationService _tokenValidationService) : IRequestHandler<ValidateTokenQuery, ValidationResult>
{

    public Task<ValidationResult> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Token))
        {
            return Task.FromResult(new ValidationResult
            {
                IsValid = false,
                Error = new ErrorModel(
                    ErrorType.BadRequest,
                    "MISSING_TOKEN",
                    "Token is missing.",
                    "The authorization token was not provided in the request headers."
                )
            });
        }

        var isValid = _tokenValidationService.ValidateToken(request.Token);

        if (isValid)
        {
            return Task.FromResult(new ValidationResult
            {
                IsValid = true,
                Message = "Token validation successful."
            });
        }
        else
        {
            return Task.FromResult(new ValidationResult
            {
                IsValid = false,
                Error = new ErrorModel(
                    ErrorType.Unauthorized,
                    "INVALID_TOKEN",
                    "Token is not valid.",
                    "The provided token failed validation."
                )
            });
        }
    }
}