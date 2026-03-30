using Farmacia.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica.Models;
using System.IO;
using ClosedXML.Excel;

namespace practica.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ReportesController : Controller
    {
        private readonly practicaContext _context;

        public ReportesController(practicaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MedicamentosVencidos()
        {
            var today = DateTime.Today;
            var data = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .Where(m => m.FechaVencimiento < today)
                .ToListAsync();
            
            ViewData["Titulo"] = "Reporte: Medicamentos Vencidos";
            return View("MedicamentosReporte", data);
        }

        public async Task<IActionResult> PorVencer30Dias()
        {
            var today = DateTime.Today;
            var soon = today.AddDays(30);
            var data = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .Where(m => m.FechaVencimiento >= today && m.FechaVencimiento <= soon)
                .ToListAsync();

            ViewData["Titulo"] = "Reporte: Medicamentos por Vencer (30 días)";
            return View("MedicamentosReporte", data);
        }

        public async Task<IActionResult> BajoStock()
        {
            var data = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .Where(m => m.Stock <= 10)
                .ToListAsync();

            ViewData["Titulo"] = "Reporte: Productos con Bajo Stock";
            return View("MedicamentosReporte", data);
        }

        public async Task<IActionResult> InventarioCompleto()
        {
            var data = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .ToListAsync();

            ViewData["Titulo"] = "Reporte: Inventario Completo";
            return View("MedicamentosReporte", data);
        }

        public async Task<IActionResult> ExportToExcel(string tipo)
        {
            var today = DateTime.Today;
            var soon = today.AddDays(30);

            var query = _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .AsQueryable();

            switch (tipo)
            {
                case "MedicamentosVencidos":
                    query = query.Where(m => m.FechaVencimiento < today);
                    break;
                case "PorVencer30Dias":
                    query = query.Where(m => m.FechaVencimiento >= today && m.FechaVencimiento <= soon);
                    break;
                case "BajoStock":
                    query = query.Where(m => m.Stock <= 10);
                    break;
            }

            var data = await query.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Medicamentos");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "ID";
                worksheet.Cell(currentRow, 2).Value = "Nombre";
                worksheet.Cell(currentRow, 3).Value = "Precio";
                worksheet.Cell(currentRow, 4).Value = "Stock";
                worksheet.Cell(currentRow, 5).Value = "Vencimiento";
                worksheet.Cell(currentRow, 6).Value = "Categoría";
                worksheet.Cell(currentRow, 7).Value = "Estante";

                foreach (var item in data)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Id;
                    worksheet.Cell(currentRow, 2).Value = item.Nombre;
                    worksheet.Cell(currentRow, 3).Value = item.Precio;
                    worksheet.Cell(currentRow, 4).Value = item.Stock;
                    worksheet.Cell(currentRow, 5).Value = item.FechaVencimiento.ToShortDateString();
                    worksheet.Cell(currentRow, 6).Value = item.Categoria?.Nombre;
                    worksheet.Cell(currentRow, 7).Value = item.Estante?.Nombre;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte_{tipo}_{DateTime.Now:yyyyMMdd}.xlsx");
                }
            }
        }
    }
}
