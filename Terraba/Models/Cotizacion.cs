using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Terraba.Models
{
    [Table("COTIZACION")]
    public class Cotizacion
    {
        [Key]
        [Column("id_cotizacion")]
        public int Id { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column("estado")]
        public string Estado { get; set; } = "Borrador";

        [Column("subtotal", TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column("iva", TypeName = "decimal(18,2)")]
        public decimal Iva { get; set; }

        [Column("total", TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Column("fecha_envio")]
        public DateTime? FechaEnvio { get; set; }

        [Column("fecha_aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Column("id_cliente")]
        public int? IdCliente { get; set; }

        [Column("id_usuario")]
        public int? IdUsuario { get; set; }

        [NotMapped]
        public string Cliente { get; set; } = string.Empty;

        [NotMapped]
        public string Medio { get; set; } = string.Empty;

        [NotMapped]
        public int Duracion { get; set; } = 1;

        [NotMapped]
        public decimal Alcance { get; set; } = 1m;

        [NotMapped]
        public string Diseno { get; set; } = string.Empty;
    }
}