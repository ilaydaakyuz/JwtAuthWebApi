using MediatR;
using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Commands;

public class LoginQuery : IRequest<LoginResult>
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

}
