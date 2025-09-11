using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ClinicaCitas.Models;

namespace ClinicaCitas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; } = default!;
        public DbSet<Medico> Medicos { get; set; } = default!;
        public DbSet<Cita> Citas { get; set; } = default!;
        public DbSet<Especialidad> Especialidades { get; set; } = default!;
    }
}
