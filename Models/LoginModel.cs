namespace MyWebApi.Models;

    public class LoginModel
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
