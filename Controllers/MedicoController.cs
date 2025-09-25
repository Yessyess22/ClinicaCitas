using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Medico")]
    public class MedicoController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        // Mostrar las citas del médico logueado
        public async Task<IActionResult> MisCitas([FromServices] ClinicaCitas.Data.ApplicationDbContext _context)
        {
            var userName = User.Identity?.Name;
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Nombre == userName);
            if (medico == null)
            {
                return View(new List<ClinicaCitas.Models.Cita>());
            }
            var citas = await _context.Citas
                .Include(c => c.Paciente)
                .Where(c => c.MedicoId == medico.MedicoId)
                .ToListAsync();
            return View(citas);
        }

        // Acción para aceptar cita
        [HttpPost]
        public async Task<IActionResult> AceptarCita(int id, [FromServices] ClinicaCitas.Data.ApplicationDbContext _context)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null && cita.Estado == "Pendiente")
            {
                cita.Estado = "Aceptada";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MisCitas");
        }

        // Acción para rechazar cita
        [HttpPost]
        public async Task<IActionResult> RechazarCita(int id, [FromServices] ClinicaCitas.Data.ApplicationDbContext _context)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null && cita.Estado == "Pendiente")
            {
                cita.Estado = "Rechazada";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MisCitas");
        }
    }
}
