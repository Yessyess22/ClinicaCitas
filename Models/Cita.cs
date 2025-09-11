using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaCitas.Models
{
    public class Cita
    {
        public int CitaId { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        public int MedicoId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        [ForeignKey("MedicoId")]
        public Medico Medico { get; set; }
    }
}
