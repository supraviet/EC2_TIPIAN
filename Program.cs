using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EC2_tipian.Data;
using EC2_tipian.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Entity Framework con SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. Configuración de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// RUTAS CORREGIDAS - Cambié "Innmuebles" por "Inmuebles"
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inmuebles}/{action=Catalogo}/{id?}"); // ← CORREGIDO AQUÍ

app.MapControllerRoute(
    name: "inmuebles",
    pattern: "Inmuebles/{action=Catalogo}/{id?}",
    defaults: new { controller = "Inmuebles" });

app.MapRazorPages();

// 3. SEED DATA - Ejecutar después de construir la app
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Asegurar que la base de datos esté creada
        context.Database.EnsureCreated();
        
        // Ejecutar seed data
        SeedData.Initialize(services);
    }
}
catch (Exception ex)
{
    var logger = app.Logger;
    logger.LogError(ex, "Error durante el seed de la base de datos");
}

app.Run();