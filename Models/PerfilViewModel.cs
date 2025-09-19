using System.ComponentModel.DataAnnotations;

namespace ClinicaCitas.Models
{
    public class PerfilViewModel
    {
    [Display(Name = "Nombre de usuario")]
    public string? NombreUsuario { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [Display(Name = "Correo electrónico")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [Display(Name = "Nombre completo")]
    public string? NombreCompleto { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\+[1-9]{1}[0-9]{1,2}[0-9]{7,12}$", ErrorMessage = "El teléfono debe incluir el código de país y solo números. Ejemplo: +521234567890")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [Required(ErrorMessage = "El CI es obligatorio.")]
    [MinLength(6, ErrorMessage = "El CI debe tener al menos 6 caracteres.")]
    [Display(Name = "CI")]
    public string? CI { get; set; }

    [Required(ErrorMessage = "El género es obligatorio.")]
    [Display(Name = "Género")]
    public string? Genero { get; set; }

    [Display(Name = "Foto de perfil")]
    [Url(ErrorMessage = "La foto de perfil debe ser una URL válida.")]
    public string? FotoPerfil { get; set; } // URL o ruta de la imagen
    }
}
