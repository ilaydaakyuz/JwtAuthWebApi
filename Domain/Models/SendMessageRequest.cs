namespace MyWebApi.Domain.Models
{
    public class SendMessageRequest
    {
        public string Message { get; set; }
        public string CorrelationId { get; set; }
    }
}