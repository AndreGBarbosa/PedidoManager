using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models;
using System.Diagnostics;

namespace PedidoManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Privacy() => View();
    }
}
