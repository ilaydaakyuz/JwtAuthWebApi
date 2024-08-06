using MediatR;
using MyWebApi.Models;

public class RegisterUserCommand : IRequest<Result>{
    public string? ClientId {get; set;}
    public string? ClientSecret {get; set;}
}