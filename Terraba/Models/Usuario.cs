using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Terraba.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("rol")]
        public string Rol { get; set; } = "vendedor";

        [Column("estado")]
        public string Estado { get; set; } = "Activo";

        [Column("intentos_fallidos")]
        public int IntentosFallidos { get; set; } = 0;

        [Column("bloqueado_hasta")]
        public DateTime? BloqueadoHasta { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("fecha_modificacion")]
        public DateTime? FechaModificacion { get; set; }
    }
}