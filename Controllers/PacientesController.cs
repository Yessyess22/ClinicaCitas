using ClinicaCitas.Data;
using ClinicaCitas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Administrador,Medico")]
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

    // OBTENER: Pacientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pacientes.ToListAsync());
        }

    // OBTENER: Pacientes/Crear
        public IActionResult Crear()
        {
            return View();
        }

    // POST: Pacientes/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

    // OBTENER: Pacientes/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            return View(paciente);
        }

    // POST: Pacientes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Paciente paciente)
        {
            if (id != paciente.PacienteId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

    // OBTENER: Pacientes/Eliminar/5
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            return View(paciente);
        }

    // POST: Pacientes/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
