using Newtonsoft.Json;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Сloudfactory.MessageBroker.Models
{
    public class Broker : IBroker
    {
        private readonly IStorage _storage;
        private readonly IClients _clients;
        int _maxAttempts = 10;        

        public Broker(IStorage storage, IClients clients)
        {
            _storage = storage;
            _clients = clients;            
        }

        /// <summary>
        /// Оправка запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Ответ запроса</returns>
        public Response SendRequest(Request request)
        {
            Console.WriteLine("Broker SendRequest "+ request.ToString());
            string key = CalculateRequestKey(request);
            
            Console.WriteLine("Broker SendRequest key:" + key);
            if (!_storage.Exists(key))
            {
                _storage.SaveRequest(key, request);
                Console.WriteLine("Broker SendRequest Такого ключа раньше не было:" + key);
            }
            else
            {
                var existingRequests = _storage.LoadRequest(key);
               _storage.SaveRequest(key, existingRequests);
            }           
            return ReceiveResponse(key);
        }
        
        /// <summary>
        /// Получить ответ запроса
        /// </summary>
        /// <param name="key">Ключ запроса</param>
        /// <returns>Ответ</returns>
        public Response ReceiveResponse(string key)
        {
            Console.WriteLine("Broker ReceiveResponse key" + key);
            int attempts = 0;

            while (attempts < _maxAttempts) // Цикл с ограничением попыток
            {
                var responseVal = _storage.LoadResponse(key);

                if (responseVal == null)
                {
                    Thread.Sleep(1000);
                    attempts++; // Увеличиваем счетчик попыток
                }
                else
                {
                    return responseVal;
                }
            }
            if (attempts >= _maxAttempts)
            {
                //TODO потом написать токен отмены 
                //_storage.DeleteRequest(key);
                throw new TimeoutException("Превышено время ожидания ответа.");               
            }

            return new Response(); // По достижении максимального количества попыток возвращаем пустой ответ или генерируем исключение
        }

        public void CollapseRequests()
        {
            // Схлопывание идентичных запросов перед отправкой
            var allRequests = _storage.GetAllRequests();

            foreach (var key in allRequests)
            {
                List<Request> requests = new List<Request>
                {
                    _storage.LoadRequest(key)
                };

                var mergedRequest = MergeRequests(requests);

                // Отправляем объединенный запрос клиентам и сохраняем ответ в хранилище
                SendMergedRequest(mergedRequest);

                // Удаляем запросы из хранилища
                foreach (var request in requests)
                {
                    _storage.DeleteRequest(CalculateRequestKey(request));
                }
            }
        }
        private string CalculateRequestKey(Request request)
        {
            return CalculateMD5Hash(request.Method + request.Path);
        }

        private string CalculateMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();                 
            }
        }

        private Request MergeRequests(List<Request> requests)
        {
            // Примерная логика объединения запросов: объединяем пути и тела запросов

            string mergedPath = requests.First().Path; // Инициализируем объединенный путь значением первого запроса

            StringBuilder mergedBody = new StringBuilder(); // Инициализируем объединенное тело запроса

            foreach (var request in requests)
            {
                mergedBody.AppendLine(request.Body); // Добавляем тело каждого запроса в объединенное тело, при необходимости можно добавить разделителей между телами запросов
            }

            Request mergedRequest = new Request
            {
                Method = requests.First().Method, // Метод может быть одинаковым у всех объединяемых запросов, берем метод первого запроса
                Path = mergedPath,
                Body = mergedBody.ToString()
            };

            return mergedRequest;
        }

        private void SendMergedRequest(Request request)
        {
            // Логика отправки объединенного запроса и сохранения ответа
            var response = SendRequestToBackend(request); // Отправка запроса бэкэнду и получение ответа

            string key = CalculateRequestKey(request); // Расчет ключа запроса для сохранения ответа в хранилище
            _storage.SaveResponse(key, response); // Сохранение ответа в хранилище
        }

        private Response SendRequestToBackend(Request request)
        {
            return new Response {
                Body = request.Body+ " "+ new Random().Next(100),
                StatusCode = 1
            }  ;
        }

        
    }
}
