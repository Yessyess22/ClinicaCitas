using System.ComponentModel.DataAnnotations;

namespace ClinicaCitas.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
