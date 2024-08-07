namespace MyWebApi.Domain.Models;
public class RegisterResult
{
    public bool isSuccess { get; set; }
    public string Message { get; set; }
    public ErrorModel error { get; set; }
    public object Data { get; set; }

}