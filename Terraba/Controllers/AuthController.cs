using Microsoft.AspNetCore.Mvc;
using Terraba.Models;

namespace Terraba.Controllers
{
    public class AuthController : Controller
    {
        private static Usuario admin = new Usuario
        {
            Correo = "admin@terraba.com",
            Password = "1234",
            Rol = "Admin"
        };

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            if (correo == admin.Correo && password == admin.Password)
            {
                HttpContext.Session.SetString("usuario", correo);
                HttpContext.Session.SetString("rol", admin.Rol);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}