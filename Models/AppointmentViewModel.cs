using System.ComponentModel.DataAnnotations;

namespace ClinicaCitas.Models
{
    public class AppointmentViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string Doctor { get; set; }
        public string Message { get; set; }
    }
}
