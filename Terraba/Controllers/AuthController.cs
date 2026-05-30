using Microsoft.AspNetCore.Mvc;
using Terraba.Data;
using Terraba.Models;

namespace Terraba.Controllers
{
    public class AuthController : Controller
    {
        private readonly MediaReachContext _context;

        public AuthController(MediaReachContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == correo);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }

            // CA-04: verificar si está bloqueado
            if (usuario.BloqueadoHasta.HasValue && usuario.BloqueadoHasta > DateTime.Now)
            {
                var minutos = (int)(usuario.BloqueadoHasta.Value - DateTime.Now).TotalMinutes + 1;
                ViewBag.Error = $"Cuenta bloqueada. Intente de nuevo en {minutos} minuto(s).";
                return View();
            }

            if (usuario.Contrasena == password)
            {
                // Login exitoso — resetear intentos
                usuario.IntentosFallidos = 0;
                usuario.BloqueadoHasta = null;
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("usuario", usuario.Email);
                HttpContext.Session.SetString("rol", usuario.Rol);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Contraseña incorrecta — sumar intento
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 5)
                {
                    usuario.BloqueadoHasta = DateTime.Now.AddMinutes(10);
                    usuario.IntentosFallidos = 0;
                    await _context.SaveChangesAsync();
                    ViewBag.Error = "Cuenta bloqueada por 10 minutos por demasiados intentos fallidos.";
                    return View();
                }

                await _context.SaveChangesAsync();
                ViewBag.Error = $"Credenciales incorrectas. Intentos fallidos: {usuario.IntentosFallidos}/5";
                return View();
            }
        }

        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaCreacion = DateTime.Now;
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                ViewBag.Exito = "Usuario creado correctamente.";
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}