using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class User
    {
        [Key]public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string Username { get; set; }

        [StringLength(10, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 10 caracteres.")]
        [RegularExpression(@"^(?! )(?!.* $)(?=.*\S).*$", ErrorMessage = "La contraseña no puede contener solo espacios en blanco.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name {  get; set; }
    }
}
