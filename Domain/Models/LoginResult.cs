namespace MyWebApi.Domain.Models;

public class LoginResult
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public ErrorModel Error { get; set; }
}
