using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EC2_tipian.Models;

namespace EC2_tipian.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // 1. CREAR ROLES
                if (!await roleManager.RoleExistsAsync("Broker"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Broker"));
                }
                if (!await roleManager.RoleExistsAsync("Cliente"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Cliente"));
                }

                // 2. CREAR USUARIOS DE PRUEBA
                if (!userManager.Users.Any())
                {
                    // Usuario Broker
                    var broker = new ApplicationUser 
                    { 
                        UserName = "broker@test.com", 
                        Email = "broker@test.com",
                        NombreCompleto = "Broker Demo",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(broker, "123");
                    await userManager.AddToRoleAsync(broker, "Broker");

                    // Usuario Cliente
                    var cliente = new ApplicationUser 
                    { 
                        UserName = "cliente@test.com", 
                        Email = "cliente@test.com",
                        NombreCompleto = "Cliente Demo",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(cliente, "123");
                    await userManager.AddToRoleAsync(cliente, "Cliente");

                    // Usuario Admin (por si acaso)
                    var admin = new ApplicationUser 
                    { 
                        UserName = "admin@test.com", 
                        Email = "admin@test.com",
                        NombreCompleto = "Administrador",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(admin, "123");
                    await userManager.AddToRoleAsync(admin, "Broker");
                }

                // 3. CREAR INMUEBLES (tu seed original)
                if (context.Inmuebles != null && !context.Inmuebles.Any())
                {
                    var inmuebles = new Inmueble[]
                    {
                        new Inmueble { Codigo = "DEP-001", Titulo = "Moderno departamento en Miraflores", Imagen = "/images/depto1.jpg", Tipo = TipoInmueble.Departamento, Ciudad = "Lima", Direccion = "Av. Larco 123", Dormitorios = 2, Banos = 2, MetrosCuadrados = 85.5, Precio = 250000, Activo = true },
                        new Inmueble { Codigo = "CASA-001", Titulo = "Acogedora casa en Surco", Imagen = "/images/casa1.jpg", Tipo = TipoInmueble.Casa, Ciudad = "Lima", Direccion = "Calle Los Pinos 456", Dormitorios = 3, Banos = 2, MetrosCuadrados = 120.0, Precio = 420000, Activo = true },
                        new Inmueble { Codigo = "OFI-001", Titulo = "Oficina ejecutiva en San Isidro", Imagen = "/images/oficina1.jpg", Tipo = TipoInmueble.Oficina, Ciudad = "Lima", Direccion = "Av. Javier Prado 789", Dormitorios = 0, Banos = 1, MetrosCuadrados = 45.0, Precio = 150000, Activo = true },
                        new Inmueble { Codigo = "LOC-001", Titulo = "Local comercial en centro comercial", Imagen = "/images/local1.jpg", Tipo = TipoInmueble.Local, Ciudad = "Lima", Direccion = "CC. Jockey Plaza, tienda 205", Dormitorios = 0, Banos = 1, MetrosCuadrados = 60.0, Precio = 300000, Activo = false }
                    };

                    context.Inmuebles.AddRange(inmuebles);
                    await context.SaveChangesAsync();
                }
            }
        }

        // Método original para compatibilidad
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Inmuebles == null || context.Inmuebles.Any()) return;

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