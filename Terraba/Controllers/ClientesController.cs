using Microsoft.AspNetCore.Mvc;

namespace Terraba.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Create() => View();
        public IActionResult Edit() => View();
        public IActionResult Details() => View();
    }
}