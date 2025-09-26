using System.ComponentModel.DataAnnotations;

namespace ClinicaCitas.Models
{
    public class Paciente
    {
    public int PacienteId { get; set; }

    // Vinculación con usuario Identity
    public string? UsuarioId { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [StringLength(20)]
        public string CI { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }
    }
}
