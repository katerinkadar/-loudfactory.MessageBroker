namespace Сloudfactory.MessageBroker.Models
{
    public interface IBroker
    {
        Response SendRequest(Request request);
        Response ReceiveResponse(string key);
        void CollapseRequests();
       
    }
}
