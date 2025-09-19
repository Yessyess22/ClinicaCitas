using Microsoft.AspNetCore.Identity;

namespace ClinicaCitas.Models
{
    public class Usuario : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? CI { get; set; }
        public string? Genero { get; set; }
        public string? FotoPerfil { get; set; }
    }
}
