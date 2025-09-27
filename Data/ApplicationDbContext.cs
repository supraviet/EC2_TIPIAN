using Microsoft.EntityFrameworkCore;
using EC2_tipian.Models;

namespace EC2_tipian.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Inmueble
            modelBuilder.Entity<Inmueble>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.HasIndex(i => i.Codigo).IsUnique();
                
                // Restricciones usando la nueva sintaxis
                entity.ToTable(t => t.HasCheckConstraint("CK_Inmueble_Precio", "Precio > 0"));
                entity.ToTable(t => t.HasCheckConstraint("CK_Inmueble_MetrosCuadrados", "MetrosCuadrados > 0"));
                
                entity.Property(i => i.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(i => i.Ciudad).IsRequired().HasMaxLength(100);
                entity.Property(i => i.Direccion).IsRequired().HasMaxLength(300);
            });

            // Configuración de Visita
            modelBuilder.Entity<Visita>(entity =>
            {
                entity.HasKey(v => v.Id);
                
                // Restricción usando nueva sintaxis
                entity.ToTable(t => t.HasCheckConstraint("CK_Visita_Fechas", "FechaInicio < FechaFin"));
                
                entity.HasOne(v => v.Inmueble)
                      .WithMany()
                      .HasForeignKey(v => v.InmuebleId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice para evitar visitas solapadas
                entity.HasIndex(v => new { v.InmuebleId, v.FechaInicio, v.FechaFin });
            });

            // Configuración de Reserva
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(r => r.Id);
                
                entity.HasOne(r => r.Inmueble)
                      .WithMany()
                      .HasForeignKey(r => r.InmuebleId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice para buscar reservas activas eficientemente
                entity.HasIndex(r => new { r.InmuebleId, r.FechaExpiracion });
            });
        }
    }
}