using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ClinicaCitas.Models;

namespace ClinicaCitas.Controllers
{
    // [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
            private readonly IUserStore<Usuario> _userStore;

            public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IUserStore<Usuario> userStore)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _userStore = userStore;
            }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError("Password", "La contraseña no puede estar vacía.");
                    return View(model);
                }
                var user = new Usuario { UserName = model.NombreUsuario, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Usuario registrado con éxito.";
                    await _userManager.AddToRoleAsync(user, "Paciente"); // Asigna el rol por defecto
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Register");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Datos inválidos. Verifica los campos e intenta nuevamente.");
            }
            return View(model);
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !string.IsNullOrEmpty(user.UserName))
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // Obtener los roles del usuario
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Administrador"))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else if (roles.Contains("Medico"))
                        {
                            return RedirectToAction("Dashboard", "Medico");
                        }
                        else if (roles.Contains("Paciente"))
                        {
                            return RedirectToAction("Dashboard", "Paciente");
                        }
                        else
                        {
                            // Si no tiene rol, redirigir al Home
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

            [Authorize]
            [HttpGet]
            public async Task<IActionResult> Perfil()
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                var model = new PerfilViewModel
                {
                    NombreUsuario = user.UserName,
                    Email = user.Email,
                    NombreCompleto = user.NombreCompleto,
                    FechaNacimiento = user.FechaNacimiento,
                    Telefono = user.Telefono,
                    Direccion = user.Direccion,
                    CI = user.CI,
                    Genero = user.Genero,
                    FotoPerfil = user.FotoPerfil
                };
                return View(model);
            }

            [Authorize]
            [HttpPost]
            public async Task<IActionResult> Perfil(PerfilViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                // Actualiza todos los campos editables del perfil
                bool updated = false;
                if (user.Email != model.Email)
                {
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email?.ToUpper();
                    updated = true;
                }
                if (user.NombreCompleto != model.NombreCompleto)
                {
                    user.NombreCompleto = model.NombreCompleto;
                    updated = true;
                }
                if (user.FechaNacimiento != model.FechaNacimiento)
                {
                    user.FechaNacimiento = model.FechaNacimiento;
                    updated = true;
                }
                if (user.Telefono != model.Telefono)
                {
                    user.Telefono = model.Telefono;
                    updated = true;
                }
                if (user.Direccion != model.Direccion)
                {
                    user.Direccion = model.Direccion;
                    updated = true;
                }
                if (user.CI != model.CI)
                {
                    user.CI = model.CI;
                    updated = true;
                }
                if (user.Genero != model.Genero)
                {
                    user.Genero = model.Genero;
                    updated = true;
                }
                if (user.FotoPerfil != model.FotoPerfil)
                {
                    user.FotoPerfil = model.FotoPerfil;
                    updated = true;
                }
                if (updated)
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View(model);
            }
    }
}
