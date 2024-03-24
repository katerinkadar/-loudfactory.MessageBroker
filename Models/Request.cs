namespace Сloudfactory.MessageBroker.Models
{
    /// <summary>Модель запроса</summary>
    public class Request
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
    }
}

