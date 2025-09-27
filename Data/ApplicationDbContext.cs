using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EC2_tipian.Models;

namespace EC2_tipian.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ CONFIGURACIÓN COMPLETA DE INMUEBLE
            builder.Entity<Inmueble>(entity =>
            {
                entity.HasIndex(i => i.Codigo).IsUnique();
                entity.Property(i => i.Precio).HasColumnType("decimal(18,2)");
                
                // Restricciones de la Pregunta 1
                entity.HasCheckConstraint("CK_Inmueble_Precio_Positivo", "Precio > 0");
                entity.HasCheckConstraint("CK_Inmueble_MetrosCuadrados_Positivo", "MetrosCuadrados > 0");
            });

            // ✅ CONFIGURACIÓN COMPLETA DE VISITA
            builder.Entity<Visita>(entity =>
            {
                // Restricción: FechaInicio < FechaFin
                entity.HasCheckConstraint("CK_Visita_Fechas_Validas", "FechaInicio < FechaFin");

                // Índice para evitar visitas solapadas (restricción de la Pregunta 1)
                entity.HasIndex(v => new { v.InmuebleId, v.FechaInicio, v.FechaFin });

                // Relación con Inmueble
                entity.HasOne(v => v.Inmueble)
                    .WithMany() // Sin navegación inversa por ahora
                    .HasForeignKey(v => v.InmuebleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Usuario (solo FK, sin navegación)
                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(v => v.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ✅ CONFIGURACIÓN COMPLETA DE RESERVA
            builder.Entity<Reserva>(entity =>
            {
                // Índice para reservas activas (restricción de la Pregunta 1)
                entity.HasIndex(r => new { r.InmuebleId, r.FechaExpiracion });

                // Relación con Inmueble
                entity.HasOne(r => r.Inmueble)
                    .WithMany() // Sin navegación inversa por ahora
                    .HasForeignKey(r => r.InmuebleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Usuario (solo FK, sin navegación)
                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(r => r.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}