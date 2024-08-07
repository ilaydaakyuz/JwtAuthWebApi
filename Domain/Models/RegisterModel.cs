namespace MyWebApi.Domain.Models;

public class RegisterModel
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}
