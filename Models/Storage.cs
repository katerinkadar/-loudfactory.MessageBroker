using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace Сloudfactory.MessageBroker.Models
{
    public class Storage : IStorage
    {
        private readonly string _storageDirectory = @"..\TestFilesAll";

        public Storage()
        {           
        }

        public void SaveRequest(string key, Request request)
        {
            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory); // Создаем директорию, если ее нет
            }
            var requestPath = Path.Combine(_storageDirectory, $"{key}.req");
            var requestJson = JsonConvert.SerializeObject(request);
            File.WriteAllText(requestPath, requestJson);
        }

        public void SaveResponse(string key, Response response)
        {
            var responsePath = Path.Combine(_storageDirectory, $"{key}.resp");
            var responseJson = JsonConvert.SerializeObject(response);
            File.WriteAllText(responsePath, responseJson);
        }

        public Request? LoadRequest(string key)
        {
            var requestPath = Path.Combine(_storageDirectory, $"{key}.req");
            if (!File.Exists(requestPath)) { return null; }
            var requestJson = File.ReadAllText(requestPath);
            return JsonConvert.DeserializeObject<Request>(requestJson);
        }

        public Response? LoadResponse(string key)
        {
            var responsePath = Path.Combine(_storageDirectory, $"{key}.resp");
            if (!File.Exists(responsePath)) { return null; }
            var responseJson = File.ReadAllText(responsePath);
            return JsonConvert.DeserializeObject<Response>(responseJson);
        }

        public void DeleteRequest(string key)
        {
            var requestPath = Path.Combine(_storageDirectory, $"{key}.req");
            File.Delete(requestPath);
        }
        public bool Exists(string key)
        {
            var requestPath = Path.Combine(_storageDirectory, $"{key}.req");
            return File.Exists(requestPath);
        }

        public string[] GetAllRequests()
        {
            return Directory.GetFiles(_storageDirectory, "*.req")
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
        }

        public void DeleteResponse(string key)
        {
            throw new NotImplementedException();
        }
    }
}
