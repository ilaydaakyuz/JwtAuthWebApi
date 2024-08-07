namespace MyWebApi.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username);
    }
}