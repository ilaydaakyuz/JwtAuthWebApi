
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Application.Attributes;
using MyWebApi.Domain.Commands;
using MyWebApi.Domain.Enums;

namespace MyWebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILogger<AuthController> _logger, IMediator _mediator) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.isSuccess)
        {
            return Ok(result.Message);
        }
        return StatusCode(GetStatusCodeFromErrorType(result.error?.ErrorType ?? ErrorType.Unknown), result.error);
    }

    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult HealthCheck()
    {
        return Ok("API is running");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Error != null)
        {
            return result.Error.ErrorType == ErrorType.Unauthorized
                ? Unauthorized(result.Error)
                : StatusCode((int)result.Error.ErrorType, result.Error);
        }

        return Ok(new { token = result.Token, userId = result.UserId });
    }


    [HttpGet("validate")]
    [CustomAuthorize]
    public async Task<IActionResult> ValidateToken()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var query = new ValidateTokenQuery { Token = token };
        var result = await _mediator.Send(query);

        if (result.IsValid)
        {
            return Ok(new { message = result.Message });
        }
        else
        {
            if (result.Error != null)
            {
                return result.Error.ErrorType == ErrorType.BadRequest
                    ? BadRequest(result.Error)
                    : Unauthorized(result.Error);
            }
            else
                return Ok(result);
        }
    }


    [HttpGet("GetUserInfoWithRedis")]
    public async Task<IActionResult> GetUserInfo(string userId)
    {
        _logger.LogInformation($"GetUserInfo method called for userId: {userId}");

        var query = new GetUserInfoQuery { UserId = userId };
        var result = await _mediator.Send(query);

        if (result.Error != null)
        {
            return result.Error.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.Error),
                _ => StatusCode((int)result.Error.ErrorType, result.Error)
            };
        }

        return Ok(result.UserInfo);
    }
    
     [HttpPost("SendMessage")]
         public async Task<IActionResult> SendMessage([FromBody] string message)
    {
        var command = new SendMessageCommand(message);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    private int GetStatusCodeFromErrorType(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.ValidationError => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
