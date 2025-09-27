using Microsoft.AspNetCore.Identity;

namespace EC2_tipian.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        // ✅ COMENTADO temporalmente - se agregará después
        // public ICollection<Visita> Visitas { get; set; } = new List<Visita>();
        // public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}