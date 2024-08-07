namespace MyWebApi.Domain.Models;

public class LogEntry : Entity
{
    public string LogLevel { get; set; }
    public int EventId { get; set; }
    public string Response { get; set; }
    public string Request { get; set; }
    public string RequestName { get; set; }
    public string Exception { get; set; }
    public DateTime Timestamp { get; set; }
    public string ElapsedTime { get; set; }
}
