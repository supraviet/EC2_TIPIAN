using Microsoft.AspNetCore.Mvc;
using EC2_tipian.Models;

namespace EC2_tipian.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                // Sesión simple - compatible con Redis después
                HttpContext.Session.SetString("UsuarioEmail", email);
                return RedirectToAction("Catalogo", "Inmuebles");
            }
            
            ViewBag.Error = "Ingresa un email válido";
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UsuarioEmail");
            return RedirectToAction("Catalogo", "Inmuebles");
        }
    }
}