using Microsoft.AspNetCore.Mvc;

namespace Terraba.Controllers
{
    public class CotizacionesController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Create() => View();
        public IActionResult Details() => View();
        public IActionResult Edit() => View();
        public IActionResult EnviarUsuario() => View();
        public IActionResult RevisionCliente() => View();
    }
}