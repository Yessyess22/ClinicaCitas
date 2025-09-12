using ClinicaCitas.Data;
using ClinicaCitas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Administrador,Medico,Paciente")]
    public class CitasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitasController(ApplicationDbContext context)
        {
            _context = context;
        }

    // OBTENER: Citas
        public async Task<IActionResult> Index()
        {
            var citas = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();
            return View(citas);
        }

    // OBTENER: Citas/Crear
        public IActionResult Crear()
        {
            ViewBag.Pacientes = _context.Pacientes.ToList();
            ViewBag.Medicos = _context.Medicos.ToList();
            return View();
        }

    // POST: Citas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cita cita)
        {
            if (ModelState.IsValid)
            {
                // Validar que no exista otra cita en la misma fecha y con el mismo médico
                var existeCita = await _context.Citas
                    .AnyAsync(c => c.MedicoId == cita.MedicoId && c.Fecha == cita.Fecha);

                if (existeCita)
                {
                    ModelState.AddModelError("", "El médico ya tiene una cita en esa fecha/hora.");
                    ViewBag.Pacientes = _context.Pacientes.ToList();
                    ViewBag.Medicos = _context.Medicos.ToList();
                    return View(cita);
                }

                _context.Citas.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cita);
        }

    // Cambiar el estado de la cita (Confirmar o Cancelar)
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            cita.Estado = estado;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    // Eliminar una cita
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(m => m.CitaId == id);

            if (cita == null) return NotFound();

            return View(cita);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
