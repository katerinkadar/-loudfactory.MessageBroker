namespace Сloudfactory.MessageBroker.Models
{
    public interface IBroker
    {
        void SendRequest(Request request);
        void ReceiveResponse(Response response);
        void CollapseRequests();
    }
}
