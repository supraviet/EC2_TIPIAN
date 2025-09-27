using Microsoft.EntityFrameworkCore;
using EC2_tipian.Models;

namespace EC2_tipian.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Inmuebles.Any()) return;

                var inmuebles = new Inmueble[]
                {
                    new Inmueble { Codigo = "DEP-001", Titulo = "Moderno departamento en Miraflores", Imagen = "/images/depto1.jpg", Tipo = TipoInmueble.Departamento, Ciudad = "Lima", Direccion = "Av. Larco 123", Dormitorios = 2, Banos = 2, MetrosCuadrados = 85.5, Precio = 250000, Activo = true },
                    new Inmueble { Codigo = "CASA-001", Titulo = "Acogedora casa en Surco", Imagen = "/images/casa1.jpg", Tipo = TipoInmueble.Casa, Ciudad = "Lima", Direccion = "Calle Los Pinos 456", Dormitorios = 3, Banos = 2, MetrosCuadrados = 120.0, Precio = 420000, Activo = true },
                    new Inmueble { Codigo = "OFI-001", Titulo = "Oficina ejecutiva en San Isidro", Imagen = "/images/oficina1.jpg", Tipo = TipoInmueble.Oficina, Ciudad = "Lima", Direccion = "Av. Javier Prado 789", Dormitorios = 0, Banos = 1, MetrosCuadrados = 45.0, Precio = 150000, Activo = true },
                    new Inmueble { Codigo = "LOC-001", Titulo = "Local comercial en centro comercial", Imagen = "/images/local1.jpg", Tipo = TipoInmueble.Local, Ciudad = "Lima", Direccion = "CC. Jockey Plaza, tienda 205", Dormitorios = 0, Banos = 1, MetrosCuadrados = 60.0, Precio = 300000, Activo = false }
                };

                context.Inmuebles.AddRange(inmuebles);
                context.SaveChanges();
            }
        }
    }
}