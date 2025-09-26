

using ClinicaCitas.Data;
using ClinicaCitas.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));


// Agregar Identity con roles
builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configurar la ruta de inicio de sesión personalizada para Identity
builder.Services.Configure<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>(Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Usuarios/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Inicializar los roles y médicos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    ClinicaCitas.Data.SeedRoles.InitializeAsync(services).GetAwaiter().GetResult();
    var context = services.GetRequiredService<ClinicaCitas.Data.ApplicationDbContext>();
    ClinicaCitas.Data.SeedMedicos.Initialize(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// Middleware personalizado: mensaje educativo y registro de accesos

app.Use(async (context, next) =>
{
    // Mensaje educativo
    Console.WriteLine("Recuerda no compartir tu contraseña.");

    // Registro de acceso en archivo
    var user = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Invitado";
    var path = context.Request.Path;
    var logLine = $"Acceso: Usuario={user}, Ruta={path}, Fecha={DateTime.Now}\n";
    var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "accesos.log");
    try
    {
        Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
        await File.AppendAllTextAsync(logPath, logLine);
    }
    catch
    {
        // Ignorar cualquier error de log y continuar
    }

    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// Ruta personalizada para /Citas
app.MapControllerRoute(
    name: "citas_index",
    pattern: "Citas",
    defaults: new { controller = "Citas", action = "Index" });

// Ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
