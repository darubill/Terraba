using Microsoft.AspNetCore.Mvc;

namespace Terraba.Controllers
{
    public class ConfiguracionController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Servicios() => View();
        public IActionResult Precios() => View();
        public IActionResult Parametros() => View();
    }
}