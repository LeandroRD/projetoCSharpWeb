using Microsoft.AspNetCore.Mvc;
using ProjetoCSharpWeb.Data;
using ProjetoCSharpWeb.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoCSharpWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
            if (usuario != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("TipoUsuario", usuario.TipoUsuario ?? "")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                // Redirecionamento conforme tipo de usuário
                if (usuario.TipoUsuario == "FORNECEDOR")
                    return RedirectToAction("Index", "Fornecedores");
                if (usuario.TipoUsuario == "CLIENTE")
                    return RedirectToAction("Index", "Clientes");
                return RedirectToAction("Index", "Usuarios");
            }
            ViewBag.Erro = "E-mail ou senha inválidos.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
