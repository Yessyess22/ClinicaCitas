using System.ComponentModel.DataAnnotations;

namespace ClinicaCitas.Models
{
    public class Medico
    {
        public int MedicoId { get; set; }

        [Required, StringLength(100)]
    public required string Nombre { get; set; }

        [Required, StringLength(100)]
    public required string Apellido { get; set; }

        public string? Telefono { get; set; }

        public int EspecialidadId { get; set; }
        public Especialidad? Especialidad { get; set; }
    }
}
