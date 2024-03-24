using System;
using System.Security.Cryptography;
using System.Text;

namespace Сloudfactory.MessageBroker.Models
{
    public class Broker : IBroker
    {
        private readonly IStorage _storage;
        private readonly IClients _clients;
        // Другие зависимости

        public Broker(IStorage storage, IClients clients)
        {
            _storage = storage;
            _clients = clients;
            // Инициализация других зависимостей
        }

        public void SendRequest(Request request)
        {
            string key = CalculateRequestKey(request);

            if (!_storage.Exists(key))
            {
                _storage.SaveRequest(key, request);
            }
            else
            {
                var existingRequests = _storage.LoadRequest(key);
               // existingRequests.Add(request);
                _storage.SaveRequest(key, existingRequests);
            }
            //// Отправка запроса клиентам и сохранение в хранилище
            //string key = CalculateRequestKey(request);

            //_storage.SaveRequest(key, request);

            //if (!_storage.Exists(key))
            //{
            //    _storage.SaveRequest(key, request);
            //}
            //else
            //{
            //    // Если запрос с таким ключом уже существует, добавляем текущий запрос в коллекцию запросов с данным ключом
            //    Request existingRequests = _storage.LoadRequest(key);
            //    existingRequests.Add(request);
            //    _storage.SaveRequest(key, existingRequests);
            //}
            //var response = GetResponse(request);
            //_storage.SaveResponse(key, response);

            //ReceiveResponse(response);
        }

        public void ReceiveResponse(Response response)
        {
            // Прием ответа от клиента и передача вызывающему
            _clients.SendResponse(response);
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
                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
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
