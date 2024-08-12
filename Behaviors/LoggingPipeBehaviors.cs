
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using MediatR;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;
using RabbitMQ.Client;

namespace MyWebApi.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>() : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();



        // Log to MongoDB
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            RequestName = $"{requestName}",
            Request = $"{JsonSerializer.Serialize(request)}",
            Response = $"{JsonSerializer.Serialize(response)}",
            ElapsedTime = $"{stopwatch.ElapsedMilliseconds} ms",
            LogLevel = "Information"
        };
        // await logEntryRepository.AddAsync(logEntry);
        SendLogToRabbitMQ(logEntry);
        return response;
    }

    private void SendLogToRabbitMQ(LogEntry logEntry)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "password" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "logQueue",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

        var message = JsonSerializer.Serialize(logEntry);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
        routingKey: "logQueue",
        basicProperties: null,
        body: body);

    }
}
