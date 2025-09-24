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
    [HttpPost]
    [Authorize(Roles = "Paciente")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgendarDesdeHome(string name, string email, string phone, string date, string department, string doctor, string message)
    {
        // Buscar el paciente por el usuario logueado
        var userName = User.Identity?.Name;
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Nombre == name || p.Telefono == phone || p.CI == userName);
        if (paciente == null)
        {
            paciente = new Paciente {
                Nombre = name,
                Apellido = "",
                Telefono = phone,
                CI = userName ?? "",
                FechaNacimiento = DateTime.Now
            };
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
        }

        // department = EspecialidadId, doctor = MedicoId
        if (!int.TryParse(department, out int especialidadId)) especialidadId = 0;
        if (!int.TryParse(doctor, out int medicoId)) medicoId = 0;
        var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.MedicoId == medicoId && m.EspecialidadId == especialidadId);
        if (medico == null)
        {
            TempData["CitaAgendada"] = "Error: Debe seleccionar un médico válido.";
            return RedirectToAction("Index", "Home");
        }

        DateTime fechaCita = DateTime.TryParse(date, out var f) ? f : DateTime.Now;

        var cita = new Cita
        {
            PacienteId = paciente.PacienteId,
            MedicoId = medico.MedicoId,
            Fecha = fechaCita,
            Estado = "Pendiente"
        };
        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();
        TempData["CitaAgendada"] = "¡Cita agendada exitosamente!";
        return RedirectToAction("Index", "Citas");
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
