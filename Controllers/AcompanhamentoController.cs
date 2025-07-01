using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoCSharpWeb.Data;
using ProjetoCSharpWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCSharpWeb.Controllers
{
    public class AcompanhamentoController : Controller
    {
        private readonly AppDbContext _context;

        public AcompanhamentoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Acompanhamento
        public async Task<IActionResult> Index(string sortOrder)
        {
            var aprovados = _context.Cotacoes.Include(c => c.Fornecedor).Where(c => c.Aprovacao == 1).AsQueryable();
            switch (sortOrder)
            {
                case "descricao":
                    aprovados = aprovados.OrderBy(c => c.Descricao);
                    break;
                case "descricao_desc":
                    aprovados = aprovados.OrderByDescending(c => c.Descricao);
                    break;
                case "fornecedor":
                    aprovados = aprovados.OrderBy(c => c.Fornecedor.Nome);
                    break;
                case "fornecedor_desc":
                    aprovados = aprovados.OrderByDescending(c => c.Fornecedor.Nome);
                    break;
                default:
                    aprovados = aprovados.OrderBy(c => c.Id);
                    break;
            }
            return View(await aprovados.ToListAsync());
        }

        // GET: Acompanhamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cotacao = await _context.Cotacoes.Include(c => c.Fornecedor).FirstOrDefaultAsync(c => c.Id == id && c.Aprovacao == 1);
            if (cotacao == null) return NotFound();
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome", cotacao.FornecedorId);
            return View(cotacao);
        }

        // POST: Acompanhamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,FornecedorId")] Cotacao cotacao)
        {
            if (id != cotacao.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var cotacaoDb = await _context.Cotacoes.FindAsync(id);
                if (cotacaoDb == null) return NotFound();
                cotacaoDb.Descricao = cotacao.Descricao;
                cotacaoDb.FornecedorId = cotacao.FornecedorId;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome", cotacao.FornecedorId);
            return View(cotacao);
        }
    }
}
