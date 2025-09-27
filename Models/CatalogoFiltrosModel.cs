using Microsoft.AspNetCore.Mvc.Rendering;

namespace EC2_tipian.Models
{
    public class CatalogoFiltrosModel
    {
        public string? Ciudad { get; set; }
        public TipoInmueble? Tipo { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public int? Dormitorios { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 6;

        // Para dropdowns
        public List<SelectListItem> Ciudades { get; set; } = new();
        public List<SelectListItem> TiposInmueble { get; set; } = new();
        public List<SelectListItem> OpcionesDormitorios { get; set; } = new();

        // Resultados
        public List<Inmueble> Inmuebles { get; set; } = new();
        public int TotalInmuebles { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalInmuebles / TamanoPagina);
    }
}