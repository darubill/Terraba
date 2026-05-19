using Microsoft.AspNetCore.Mvc;

namespace Terraba.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        public IActionResult Reportes()
        {
            return View();
        }
    }
}