using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Terraba.Models
{
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int Id { get; set; }
        // Contact name in the database is stored in column 'contacto'
        [Column("contacto")]
        public string Nombre { get; set; } = string.Empty;

        // Company name in the database is stored in column 'nombre_empresa'
        [Column("nombre_empresa")]
        public string Empresa { get; set; } = string.Empty;

        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("estado")]
        public string Estado { get; set; } = "Activo";

        [Column("fecha_creacion")]
        public System.DateTime FechaCreacion { get; set; } = System.DateTime.Now;

        [Column("fecha_modificacion")]
        public System.DateTime? FechaModificacion { get; set; }
    }
}