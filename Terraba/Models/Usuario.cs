using System.ComponentModel.DataAnnotations;

namespace Terraba.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }

        public string Rol { get; set; } // Admin / Cliente
    }
}