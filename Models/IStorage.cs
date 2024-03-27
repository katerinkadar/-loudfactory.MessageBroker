namespace Сloudfactory.MessageBroker.Models
{
    public interface IStorage
    {
        void SaveRequest(string key, Request request);
        void SaveResponse(string key, Response response);
        Request? LoadRequest(string key);
        Response LoadResponse(string key);
        void DeleteRequest(string key);
        void DeleteResponse(string key);
        bool Exists(string key);
        string[] GetAllRequests();
    }
}