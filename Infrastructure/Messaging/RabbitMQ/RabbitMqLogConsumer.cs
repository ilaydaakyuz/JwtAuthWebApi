
using System.Text.Json;
using MyWebApi.Domain.Models;

namespace MyWebApi.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqLogConsumer(Domain.Interfaces.IGenericRepository<LogEntry> _logEntryRepository)
    {
        public async Task ProcessLogAsync(string message)
        {
            var logEntry = JsonSerializer.Deserialize<LogEntry>(message);
            if (logEntry != null)
            {
                await _logEntryRepository.AddAsync(logEntry);
            }
        }

    }
}