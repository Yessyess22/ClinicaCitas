using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ClinicaCitas.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Paciente")]
    public class PacienteController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public PacienteController(Data.ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Historial()
        {
            var usuarioId = _userManager.GetUserId(User);
            var pacientes = await _context.Pacientes.Where(p => p.UsuarioId == usuarioId).ToListAsync();
            if (!pacientes.Any())
            {
                return View(new List<Cita>());
            }
            var pacienteIds = pacientes.Select(p => p.PacienteId).ToList();
            var citas = await _context.Citas
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Where(c => pacienteIds.Contains(c.PacienteId))
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
            return View(citas);
        }
    }
}
