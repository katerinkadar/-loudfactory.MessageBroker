using System.Net.Sockets;
using System.Text;

namespace Сloudfactory.MessageBroker.Models
{
    public class Clients : IClients
    {
        // Реализация метода интерфейса IClients
        public void SendResponse(Response response)
        {
            // Код для отправки ответа клиенту (например, по TCP/IP, HTTP и т. д.)
            // Пример кода отправки ответа по TCP/IP
            TcpClient client = new TcpClient();
            client.Connect("localhost", 8080);

            NetworkStream stream = client.GetStream();

            // Отправка имени файла
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(response.FileName);
            byte[] fileNameLengthBytes = BitConverter.GetBytes(fileNameBytes.Length);
            stream.Write(fileNameLengthBytes, 0, fileNameLengthBytes.Length);
            stream.Write(fileNameBytes, 0, fileNameBytes.Length);

            // Отправка данных файла
            byte[] fileData = File.ReadAllBytes(response.FileName);
            stream.Write(fileData, 0, fileData.Length);

            // Закрытие соединения
            client.Close();
        }
    }
}
