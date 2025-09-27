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

// 2. ✅ CONFIGURACIÓN DE IDENTITY CORREGIDA
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    
    // Configuración simplificada para pruebas
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// .AddDefaultUI(); // Eliminado porque no está disponible en esta configuración

// 3. Configuración de sesiones (para Pregunta 4)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "Inmobiliaria.Session";
});

// 4. Configuración de Redis (para Pregunta 4)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Inmobiliaria_";
});

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

// ✅ Orden CORRECTO de middlewares
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// RUTAS
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inmuebles}/{action=Catalogo}/{id?}");

app.MapControllerRoute(
    name: "inmuebles",
    pattern: "Inmuebles/{action=Catalogo}/{id?}",
    defaults: new { controller = "Inmuebles" });

app.MapRazorPages();

// 5. Seed data
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        context.Database.EnsureCreated();
        await SeedData.InitializeAsync(services, userManager, roleManager);
    }
}
catch (Exception ex)
{
    var logger = app.Logger;
    logger.LogError(ex, "Error durante el seed de la base de datos");
}

app.Run();