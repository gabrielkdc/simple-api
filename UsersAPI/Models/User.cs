using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class User
    {
        [Key]public int Id { get; set; }
        public string Username { get; set; }

        [StringLength(10, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 10 caracteres.")]
        public string Password { get; set; }
        public string Name {  get; set; }
    }
}
