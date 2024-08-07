using MediatR;
using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Commands;

public class GetUserInfoQuery : IRequest<UserInfoResult>
{
    public string UserId { get; set; }
}
