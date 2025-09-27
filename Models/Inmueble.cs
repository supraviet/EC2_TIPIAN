using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC2_tipian.Models
{
    public class Inmueble
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        public string? Imagen { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public TipoInmueble Tipo { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(50, ErrorMessage = "La ciudad no puede exceder 50 caracteres")]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Los dormitorios no pueden ser negativos")]
        public int Dormitorios { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Los baños no pueden ser negativos")]
        public int Banos { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Los metros cuadrados deben ser mayores a 0")]
        public double MetrosCuadrados { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public bool Activo { get; set; } = true;

        // ✅ COMENTADO temporalmente - se agregará después
        // public ICollection<Visita> Visitas { get; set; } = new List<Visita>();
        // public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }

    public enum TipoInmueble
    {
        Departamento,
        Casa,
        Oficina,
        Local
    }
}