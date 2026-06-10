using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terraba.Data;
using Terraba.Models;

namespace Terraba.Controllers
{
    public class CotizacionesController : Controller
    {
        private readonly MediaReachContext _context;
        private readonly Microsoft.Extensions.Logging.ILogger<CotizacionesController> _logger;

        public CotizacionesController(MediaReachContext context, Microsoft.Extensions.Logging.ILogger<CotizacionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Clientes = await _context.Clientes.ToListAsync();
                var q = _context.Cotizaciones.OrderByDescending(c => c.Fecha);
                try
                {
                    // log the SQL EF will execute (works in EF Core)
                    _logger?.LogDebug("EF SQL: {sql}", q.ToQueryString());
                }
                catch { }

                var list = await q.ToListAsync();

                // Map IdCliente to actual client display name (Empresa if available, otherwise Nombre)
                var clients = await _context.Clientes.ToListAsync();
                var nameMap = new Dictionary<int, string>();
                foreach (var c in clients)
                {
                    nameMap[c.Id] = !string.IsNullOrEmpty(c.Empresa) ? c.Empresa : c.Nombre;
                }

                foreach (var cot in list)
                {
                    if (cot.IdCliente.HasValue && nameMap.TryGetValue(cot.IdCliente.Value, out var display))
                    {
                        cot.Cliente = display;
                    }
                    else
                    {
                        cot.Cliente = string.Empty;
                    }
                }

                return View(list);
            }
            catch (System.Exception ex)
            {
                // Log full exception and return an empty view so the app doesn't crash
                _logger?.LogError(ex, "Failed to load Cotizaciones. Exception will be shown to developer.");
                TempData["ErrorMessage"] = "Error loading cotizaciones. Check logs for details.";
                return View(new System.Collections.Generic.List<Terraba.Models.Cotizacion>());
            }
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = await SafeGetClientesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [FromForm] string Cliente,
            [FromForm] string Medio,
            [FromForm] int Duracion,
            [FromForm] decimal Alcance,
            [FromForm] string Diseno,
            [FromForm] decimal Total,
            [FromForm] int? IdCliente)
        {
            // Basic validation
            if (Total <= 0)
            {
                ModelState.AddModelError(string.Empty, "Debe calcular un total antes de guardar la cotización.");
                // return view with previous inputs filled
                var vm = new Cotizacion { Cliente = Cliente, Medio = Medio, Duracion = Duracion, Alcance = Alcance, Diseno = Diseno, Total = Total, IdCliente = IdCliente };
                ViewBag.Clientes = await SafeGetClientesAsync();
                return View(vm);
            }

            try
            {
                var cot = new Cotizacion
                {
                    Fecha = System.DateTime.Now,
                    Subtotal = Total,
                    Iva = 0m,
                    Total = Total,
                    // Must match allowed values in CHECK constraint CK_COTIZACION_ESTADO
                    Estado = "Borrador",
                    IdUsuario = 1,
                    IdCliente = IdCliente ?? 1
                };

                // The Cotizacion table does not have columns for Medio/Diseno/Alcance/Duracion.
                // These are kept as NotMapped properties and won't be persisted for existing rows.

                _context.Cotizaciones.Add(cot);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                _logger?.LogError(dbEx, "Failed to save Cotizacion.");
                if (dbEx.InnerException != null)
                {
                    _logger?.LogError(dbEx.InnerException, "Inner exception while saving Cotizacion.");
                }
                ModelState.AddModelError(string.Empty, "Error saving cotizacion. Check logs for details.");
                var vm = new Cotizacion { Cliente = Cliente, Medio = Medio, Duracion = Duracion, Alcance = Alcance, Diseno = Diseno, Total = Total, IdCliente = IdCliente };
                ViewBag.Clientes = await SafeGetClientesAsync();
                return View(vm);
            }
        }

        // Helper: load clients safely and log failures so UI doesn't crash when DB schema is different
        private async Task<List<Cliente>> SafeGetClientesAsync()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Failed to load Clientes; returning empty list.");
                TempData["ErrorMessage"] = "No se pudieron cargar los clientes desde la base de datos.";
                return new List<Cliente>();
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Cotizaciones.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Cotizaciones.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cotizacion model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.Cotizaciones.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Cotizaciones.FindAsync(id);
            if (item != null)
            {
                _context.Cotizaciones.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EnviarUsuario() => View();
        public IActionResult RevisionCliente() => View();
    }
}
