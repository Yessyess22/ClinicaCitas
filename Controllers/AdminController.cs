using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ClinicaCitas.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        // Vista para listar usuarios y asignar roles
        public async Task<IActionResult> Usuarios()
        {
            var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<ClinicaCitas.Models.Usuario>)) as UserManager<ClinicaCitas.Models.Usuario>;
            if (userManager == null)
            {
                ViewBag.UserRoles = new List<(ClinicaCitas.Models.Usuario User, IList<string> Roles)>();
                return View();
            }
            var users = userManager.Users.ToList();
            var userRoles = new List<(ClinicaCitas.Models.Usuario User, IList<string> Roles)>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add((user, roles));
            }
            ViewBag.UserRoles = userRoles;
            return View();
        }

        // Acción para asignar rol a un usuario
        [HttpPost]
        public async Task<IActionResult> AsignarRol(string userId, string rol)
        {
            var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<ClinicaCitas.Models.Usuario>)) as UserManager<ClinicaCitas.Models.Usuario>;
            if (userManager != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null && !string.IsNullOrEmpty(rol))
                {
                    // Elimina todos los roles actuales antes de asignar el nuevo
                    var rolesActuales = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, rolesActuales);
                    await userManager.AddToRoleAsync(user, rol);
                    TempData["SuccessMessage"] = $"Rol '{rol}' asignado correctamente.";
                }
            }
            return RedirectToAction("Usuarios");
        }

        // Acción para eliminar un rol de un usuario
        [HttpPost]
        public async Task<IActionResult> EliminarRol(string userId, string rol)
        {
            var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<ClinicaCitas.Models.Usuario>)) as UserManager<ClinicaCitas.Models.Usuario>;
            if (userManager != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null && !string.IsNullOrEmpty(rol))
                {
                    await userManager.RemoveFromRoleAsync(user, rol);
                    TempData["SuccessMessage"] = $"Rol '{rol}' eliminado correctamente.";
                }
            }
            return RedirectToAction("Usuarios");
        }
    }
}
