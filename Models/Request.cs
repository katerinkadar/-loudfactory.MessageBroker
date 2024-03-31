namespace Сloudfactory.MessageBroker.Models
{
    /// <summary>Модель запроса</summary>
    public class Request
    {
        /// <summary>
        /// Метод
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Путь
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Тело запроса
        /// </summary>
        public string Body { get; set; }
        public override string ToString()
        {
            return $"Метод: {Method}, Путь: {Path}, Тело: {Body}";
        }
    }
}

