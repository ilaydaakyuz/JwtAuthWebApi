namespace MyWebApi.Domain.Interfaces
{
    public interface ITokenValidationService
    {
        bool ValidateToken(string token);
    }
}