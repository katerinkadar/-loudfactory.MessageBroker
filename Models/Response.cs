namespace Сloudfactory.MessageBroker.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
        public string FileName { get; internal set; }
    }
}
