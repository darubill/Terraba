using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Terraba.Data;
using Terraba.Models;
using Microsoft.EntityFrameworkCore;


namespace Terraba.Controllers
{
    public class ClientesController : Controller
    {
        private readonly MediaReachContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(MediaReachContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var list = await SafeGetClientesAsync();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Cliente model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.Clientes.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _context.Clientes.FindAsync(id);
                if (item == null) return NotFound();
                return View(item);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Failed to load Cliente for Edit. Returning NotFound.");
                return NotFound();
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _context.Clientes.FindAsync(id);
                if (item == null) return NotFound();
                return View(item);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Failed to load Cliente for Details. Returning NotFound.");
                return NotFound();
            }
        }

        private async Task<List<Models.Cliente>> SafeGetClientesAsync()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Failed to load Clientes; returning empty list.");
                TempData["ErrorMessage"] = "No se pudieron cargar los clientes desde la base de datos.";
                return new List<Models.Cliente>();
            }
        }
    }
}