
using System.Diagnostics;
using System.Text.Json;
using MediatR;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;

namespace MyWebApi.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>(
    IGenericRepository<LogEntry> logEntryRepository) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : class
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
        await logEntryRepository.AddAsync(logEntry);

        return response;
    }

}
