using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyWebApi.Domain.Commands;


namespace MyWebApi.Application.Attributes;
public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizationFilter))
    {
    }
}

internal class CustomAuthorizationFilter(IMediator _mediator) : IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        var query = new ValidateTokenQuery { Token = token };
        var result = await _mediator.Send(query);

        if (!result.IsValid)
        {
            context.Result = result.Error != null
                ? new ObjectResult(result.Error) { StatusCode = (int)result.Error.ErrorType }
                : new UnauthorizedResult();
            return;
        }

        await next();
    }
}

