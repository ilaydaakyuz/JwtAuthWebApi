using MyWebApi.Models;

public class UserInfoResult
{
    public Dictionary<string, string>? UserInfo { get; set; }
    public ErrorModel? Error { get; set; }
}