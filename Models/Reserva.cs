using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC2_tipian.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public int InmuebleId { get; set; }

        [Required]
        public string UsuarioEmail  { get; set; } = string.Empty; // FK como string

        [Required]
        public DateTime FechaExpiracion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // ✅ RELACIÓN con Inmueble (sin circularidad)
        [ForeignKey("InmuebleId")]
        public Inmueble? Inmueble { get; set; }

        // ✅ NO incluir propiedad de navegación a ApplicationUser por ahora
    }
}