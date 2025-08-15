using Microsoft.AspNetCore.Mvc;

namespace PedidoManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Privacy() => View();
    }
}
