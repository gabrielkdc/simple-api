using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class Login
    {

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; }
    }
}
