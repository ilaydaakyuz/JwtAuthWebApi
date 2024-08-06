using MediatR;

public class GetUserInfoQuery : IRequest<UserInfoResult>
{
    public string? UserId { get; set; }
}