using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Сloudfactory.MessageBroker.Models;

namespace Сloudfactory.MessageBroker.Controllers
{
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
            _broker.SendRequest(request); // Отправка запроса брокеру

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            _broker.CollapseRequests(); // Схлопывание запросов и отправка объединенных запросов

            return Ok();
        }

        // Другие методы контроллера
        // GET: HttpTrafficHandler
        public ActionResult Index()
        {
            return View();
        }

        // GET: HttpTrafficHandler/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HttpTrafficHandler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HttpTrafficHandler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HttpTrafficHandler/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HttpTrafficHandler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HttpTrafficHandler/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HttpTrafficHandler/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
