using MediatR;
using MyWebApi.Models;

public class LoginQuery : IRequest<LoginResult>{
public string? ClientId{get; set;}
public string? ClientSecret{get; set;}

}