using Farmacia.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica.Models;
using practica.ViewModels;
using System.Diagnostics;

namespace practica.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly practicaContext _context;

        public HomeController(practicaContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult PublicPortal()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Administrador"))
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "Medicamentoes");
            }
            return View();
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrador"))
            {
                var today = DateTime.Today;
                var soon = today.AddDays(30);

                var dashboard = new DashboardViewModel
                {
                    TotalMedicamentos = await _context.Medicamentos.CountAsync(),
                    MedicamentosVencidos = await _context.Medicamentos.CountAsync(m => m.FechaVencimiento < today),
                    MedicamentosPorVencer = await _context.Medicamentos.CountAsync(m => m.FechaVencimiento >= today && m.FechaVencimiento <= soon),
                    ProductosBajoStock = await _context.Medicamentos.CountAsync(m => m.Stock <= 10),
                    TotalCategorias = await _context.Categorias.CountAsync(),
                    TotalEstantes = await _context.Estantes.CountAsync()
                };

                // Chart Data: Stocks per category
                var stats = await _context.Medicamentos
                    .Include(m => m.Categoria)
                    .GroupBy(m => m.Categoria!.Nombre)
                    .Select(g => new { Nombre = g.Key, Conteo = g.Count() })
                    .ToListAsync();

                foreach (var s in stats)
                {
                    dashboard.CategoriasNombres.Add(s.Nombre ?? "Sin Categoría");
                    dashboard.CategoriasConteo.Add(s.Conteo);
                }

                return View(dashboard);
            }

            return RedirectToAction("Index", "Medicamentoes");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
