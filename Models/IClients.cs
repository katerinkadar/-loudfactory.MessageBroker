namespace Сloudfactory.MessageBroker.Models
{
    public interface IClients
    {
        void SendResponse(Response response);
    }
}