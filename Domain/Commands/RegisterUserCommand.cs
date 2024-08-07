using MediatR;
using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Commands;

public class RegisterUserCommand : IRequest<RegisterResult>
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
