namespace MyWebApi.Domain.Models;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public ErrorModel Error { get; set; }
}
