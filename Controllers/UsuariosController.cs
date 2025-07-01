using Microsoft.AspNetCore.Mvc;
using ProjetoCSharpWeb.Data;
using ProjetoCSharpWeb.Models;
using System.Linq;

namespace ProjetoCSharpWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {
            // Sempre define o TipoUsuario como 'ADM' antes de validar
            usuario.TipoUsuario = "ADM";
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                TempData["success"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                _context.SaveChanges();
                TempData["success"] = "Alteração feita com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            TempData["success"] = "Usuário excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
