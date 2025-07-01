using Microsoft.AspNetCore.Mvc;
using ProjetoCSharpWeb.Data;
using ProjetoCSharpWeb.Models;
using System.Linq;

namespace ProjetoCSharpWeb.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly AppDbContext _context;
        public FornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var fornecedores = _context.Fornecedores.AsQueryable();

            // Filtra por fornecedor logado, se for o caso
            if (User.HasClaim("TipoUsuario", "FORNECEDOR"))
            {
                var emailFornecedor = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(emailFornecedor))
                {
                    fornecedores = fornecedores.Where(f => f.Email == emailFornecedor);
                }
            }

            return View(fornecedores.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Fornecedor fornecedor)
        {
            var senha = Request.Form["Senha"].ToString();
            if (ModelState.IsValid)
            {
                _context.Fornecedores.Add(fornecedor);
                _context.SaveChanges();
                // Inserir também na tabela de usuários
                var usuario = new Usuario
                {
                    Nome = fornecedor.Nome,
                    Email = fornecedor.Email,
                    Senha = senha,
                    TipoUsuario = "FORNECEDOR"
                };
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                TempData["success"] = "Fornecedor cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        public IActionResult Edit(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor == null) return NotFound();
            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Fornecedor fornecedor)
        {
            if (id != fornecedor.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(fornecedor);
                _context.SaveChanges();
                TempData["success"] = "Fornecedor atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        public IActionResult Delete(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor == null) return NotFound();
            return View(fornecedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor == null) return NotFound();
            _context.Fornecedores.Remove(fornecedor);
            _context.SaveChanges();
            TempData["success"] = "Fornecedor excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
