using Microsoft.AspNetCore.Mvc;
using ProjetoCSharpWeb.Data;
using ProjetoCSharpWeb.Models;
using System.Linq;

namespace ProjetoCSharpWeb.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clientes = _context.Clientes.ToList();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            var senha = Request.Form["Senha"].ToString();
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                // Inserir também na tabela de usuários
                var usuario = new Usuario
                {
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Senha = senha,
                    TipoUsuario = "CLIENTE"
                };
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                TempData["success"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                _context.SaveChanges();
                TempData["success"] = "Cliente atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            TempData["success"] = "Cliente excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
