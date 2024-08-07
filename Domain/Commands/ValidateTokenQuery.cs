using MediatR;
using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Commands;

public class ValidateTokenQuery : IRequest<ValidationResult>
{
    public string Token { get; set; }
}
