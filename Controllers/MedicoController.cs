using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Medico")]
    public class MedicoController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
