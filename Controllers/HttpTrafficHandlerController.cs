using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Сloudfactory.MessageBroker.Models;

namespace Сloudfactory.MessageBroker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HttpTrafficHandlerController : Controller
    {
        private readonly IBroker _broker;
        private readonly IStorage _storage;
        private readonly IClients _clients;

        public HttpTrafficHandlerController(IBroker broker, IStorage storage, IClients clients)
        {
            _broker = broker;
            _storage = storage;
            _clients = clients;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Request request)
        {
           var responseVal = _broker.SendRequest(request); // Отправка запроса брокеру

            if (responseVal != null)
            {
                return Ok(responseVal?.Body?.ToString());
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }     
    }
}
