using Microsoft.AspNetCore.Mvc;

namespace Terraba.Controllers
{
    public class NotificacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}