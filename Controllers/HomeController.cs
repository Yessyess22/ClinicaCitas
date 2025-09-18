using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClinicaCitas.Models;

namespace ClinicaCitas.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View("AccessDenied");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult ServiceDetails()
    {
        return View();
    }

    public IActionResult Appointment()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Appointment(ClinicaCitas.Models.AppointmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

    // Configuración de correo (reemplaza con tus datos reales)
    var toEmail = "contact@example.com"; // Cambia esto por tu correo real
        var subject = "Online Appointment Form";
        var body = $@"<b>Name:</b> {model.Name}<br/>
<b>Email:</b> {model.Email}<br/>
<b>Phone:</b> {model.Phone}<br/>
<b>Appointment Date:</b> {model.Date}<br/>
<b>Department:</b> {model.Department}<br/>
<b>Doctor:</b> {model.Doctor}<br/>
<b>Message:</b> {model.Message}";

        try
        {
            using (var message = new System.Net.Mail.MailMessage())
            {
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new System.Net.Mail.MailAddress(model.Email, model.Name);

                using (var smtp = new System.Net.Mail.SmtpClient("smtp.example.com", 587)) // Cambia smtp.example.com y puerto
                {
                    smtp.Credentials = new System.Net.NetworkCredential("usuario", "contraseña"); // Cambia usuario y contraseña
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }
            ViewBag.Message = "¡Cita enviada correctamente!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al enviar el correo: " + ex.Message);
            return View(model);
        }
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ClinicaCitas.Models.ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

    // Configuración de correo (reemplaza con tus datos reales)
    var toEmail = "contact@example.com"; // Cambia esto por tu correo real
        var subject = "Contacto desde el sitio web";
        var body = $@"<b>Name:</b> {model.Name}<br/>
<b>Email:</b> {model.Email}<br/>
<b>Phone:</b> {model.Phone}<br/>
<b>Message:</b> {model.Message}";

        try
        {
            using (var message = new System.Net.Mail.MailMessage())
            {
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new System.Net.Mail.MailAddress(model.Email, model.Name);

                using (var smtp = new System.Net.Mail.SmtpClient("smtp.example.com", 587)) // Cambia smtp.example.com y puerto
                {
                    smtp.Credentials = new System.Net.NetworkCredential("usuario", "contraseña"); // Cambia usuario y contraseña
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }
            ViewBag.Message = "¡Mensaje enviado correctamente!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al enviar el correo: " + ex.Message);
            return View(model);
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
