using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Paciente")]
    public class PacienteController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
