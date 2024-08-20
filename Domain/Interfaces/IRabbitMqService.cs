namespace MyWebApi.Domain.Interfaces
{
    public interface IRabbitMqService
    {
        Task<string> SendAndReceiveMessageAsync(string message, string correlationId);

    }
}