using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using EC2_tipian.Models;
using EC2_tipian.Data;

namespace EC2_tipian.Controllers
{
    [Route("Inmuebles")]
    public class InmueblesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InmueblesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Inmuebles/Catalogo
        [HttpGet("Catalogo")]
        [HttpGet("")] // También responde a /Inmuebles
        public async Task<IActionResult> Catalogo(CatalogoFiltrosModel filtros)
        {
            // Query base - solo inmuebles activos
            var query = _context.Inmuebles.Where(i => i.Activo);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtros.Ciudad))
                query = query.Where(i => i.Ciudad.Contains(filtros.Ciudad));

            if (filtros.Tipo.HasValue)
                query = query.Where(i => i.Tipo == filtros.Tipo.Value);

            if (filtros.PrecioMin.HasValue)
                query = query.Where(i => i.Precio >= filtros.PrecioMin.Value);

            if (filtros.PrecioMax.HasValue)
                query = query.Where(i => i.Precio <= filtros.PrecioMax.Value);

            if (filtros.Dormitorios.HasValue)
                query = query.Where(i => i.Dormitorios >= filtros.Dormitorios.Value);

            // Validación de rango de precios
            if (filtros.PrecioMin.HasValue && filtros.PrecioMax.HasValue && 
                filtros.PrecioMin > filtros.PrecioMax)
            {
                ModelState.AddModelError("PrecioMax", "El precio máximo no puede ser menor al precio mínimo");
            }

            // Obtener total antes de paginación
            filtros.TotalInmuebles = await query.CountAsync();

            // Asegurar que la página esté dentro del rango válido
            if (filtros.Pagina < 1) filtros.Pagina = 1;
            
            // Calcular total de páginas usando la propiedad existente del modelo
            int totalPaginas = filtros.TotalPaginas;
            if (filtros.Pagina > totalPaginas && totalPaginas > 0)
                filtros.Pagina = totalPaginas;

            // CORRECCIÓN: Primero obtener los datos sin ordenamiento decimal
            var inmueblesQuery = await query
                .Skip((filtros.Pagina - 1) * filtros.TamanoPagina)
                .Take(filtros.TamanoPagina)
                .ToListAsync();

            // Luego ordenar en memoria (client-side)
            var inmuebles = inmueblesQuery.OrderBy(i => i.Precio).ToList();

            filtros.Inmuebles = inmuebles;

            // Llenar dropdowns
            await LlenarDropdowns(filtros);

            return View(filtros);
        }

        // GET: /Inmuebles/Detalle/5
        [HttpGet("Detalle/{id:int}")]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
                return NotFound();

            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.Id == id && i.Activo);

            if (inmueble == null)
                return NotFound();

            return View(inmueble);
        }

        // GET: /Inmuebles/Buscar (alternativa para búsquedas)
        [HttpGet("Buscar")]
        public IActionResult Buscar()
        {
            return RedirectToAction("Catalogo");
        }

        // POST: /Inmuebles/Buscar (para formularios POST)
        [HttpPost("Buscar")]
        public IActionResult Buscar(CatalogoFiltrosModel filtros)
        {
            // Redirigir a Catalogo con los filtros como query string
            return RedirectToAction("Catalogo", new 
            { 
                Ciudad = filtros.Ciudad,
                Tipo = filtros.Tipo,
                PrecioMin = filtros.PrecioMin,
                PrecioMax = filtros.PrecioMax,
                Dormitorios = filtros.Dormitorios,
                Pagina = 1
            });
        }

        private async Task LlenarDropdowns(CatalogoFiltrosModel model)
        {
            // Ciudades únicas
            var ciudades = await _context.Inmuebles
                .Where(i => i.Activo)
                .Select(i => i.Ciudad)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            model.Ciudades = ciudades.Select(c => new SelectListItem { Value = c, Text = c }).ToList();
            model.Ciudades.Insert(0, new SelectListItem { Value = "", Text = "Todas las ciudades" });

            // Tipos de inmueble
            model.TiposInmueble = Enum.GetValues(typeof(TipoInmueble))
                .Cast<TipoInmueble>()
                .Select(t => new SelectListItem { 
                    Value = ((int)t).ToString(), 
                    Text = t.ToString() 
                })
                .ToList();
            model.TiposInmueble.Insert(0, new SelectListItem { Value = "", Text = "Todos los tipos" });

            // Opciones de dormitorios (ya inicializadas en el modelo, pero las actualizamos)
            if (!model.OpcionesDormitorios.Any())
            {
                model.OpcionesDormitorios = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Cualquier cantidad" },
                    new SelectListItem { Value = "1", Text = "1+ dormitorios" },
                    new SelectListItem { Value = "2", Text = "2+ dormitorios" },
                    new SelectListItem { Value = "3", Text = "3+ dormitorios" },
                    new SelectListItem { Value = "4", Text = "4+ dormitorios" },
                    new SelectListItem { Value = "5", Text = "5+ dormitorios" }
                };
            }
        }
    }
}