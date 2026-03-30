using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using practica.Data;
using practica.Models;

namespace practica.Controllers
{
    public class MedicamentoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicamentoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicamentoes
        // SE AGREGÓ EL PARÁMETRO buscarNombre PARA EL FILTRADO
        public async Task<IActionResult> Index(string buscarNombre)
        {
            // Iniciamos la consulta incluyendo las relaciones
            var medicamentos = _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .AsQueryable();

            // Lógica de filtrado por nombre
            if (!string.IsNullOrEmpty(buscarNombre))
            {
                medicamentos = medicamentos.Where(s => s.Nombre.Contains(buscarNombre));
            }

            // Guardamos el término buscado para que no se borre del input al recargar
            ViewData["FiltroActual"] = buscarNombre;

            return View(await medicamentos.ToListAsync());
        }

        // GET: Medicamentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var medicamento = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null) return NotFound();

            return View(medicamento);
        }

        // GET: Medicamentoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Stock,FechaVencimiento,Estado,CategoriaId,EstanteId")] Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", medicamento.CategoriaId);
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre", medicamento.EstanteId);
            return View(medicamento);
        }

        // GET: Medicamentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null) return NotFound();

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", medicamento.CategoriaId);
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre", medicamento.EstanteId);
            return View(medicamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Stock,FechaVencimiento,Estado,CategoriaId,EstanteId")] Medicamento medicamento)
        {
            if (id != medicamento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoExists(medicamento.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", medicamento.CategoriaId);
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre", medicamento.EstanteId);
            return View(medicamento);
        }

        // GET: Medicamentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var medicamento = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null) return NotFound();

            return View(medicamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento != null)
            {
                _context.Medicamentos.Remove(medicamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }
    }
}