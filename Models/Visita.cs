using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC2_tipian.Models
{
    public class Visita
    {
        public int Id { get; set; }

        [Required]
        public int InmuebleId { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty; // FK como string

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateTime FechaFin { get; set; }

        [Required]
        public EstadoVisita Estado { get; set; } = EstadoVisita.Solicitada;

        [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
        public string? Notas { get; set; }

        // ✅ RELACIÓN con Inmueble (sin circularidad)
        [ForeignKey("InmuebleId")]
        public Inmueble? Inmueble { get; set; }

        // ✅ NO incluir propiedad de navegación a ApplicationUser por ahora
        // Se manejará via queries con joins
    }

    public enum EstadoVisita
    {
        Solicitada,
        Confirmada,
        Cancelada
    }
}