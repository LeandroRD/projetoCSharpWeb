using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoCSharpWeb.Data;
using ProjetoCSharpWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCSharpWeb.Controllers
{
    public class CotacoesController : Controller
    {
        private readonly AppDbContext _context;

        public CotacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cotacoes
        public async Task<IActionResult> Index(string sortOrder)
        {
            var cotacoes = _context.Cotacoes.Include(c => c.Fornecedor).AsQueryable();

            // Filtra por fornecedor logado, se for o caso
            if (User.HasClaim("TipoUsuario", "FORNECEDOR"))
            {
                var emailFornecedor = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(emailFornecedor))
                {
                    cotacoes = cotacoes.Where(c => c.Fornecedor.Email == emailFornecedor);
                }
            }

            switch (sortOrder)
            {
                case "descricao":
                    cotacoes = cotacoes.OrderBy(c => c.Descricao);
                    break;
                case "descricao_desc":
                    cotacoes = cotacoes.OrderByDescending(c => c.Descricao);
                    break;
                case "fornecedor":
                    cotacoes = cotacoes.OrderBy(c => c.Fornecedor.Nome);
                    break;
                case "fornecedor_desc":
                    cotacoes = cotacoes.OrderByDescending(c => c.Fornecedor.Nome);
                    break;
                case "valor":
                    cotacoes = cotacoes.OrderBy(c => c.Valor);
                    break;
                case "valor_desc":
                    cotacoes = cotacoes.OrderByDescending(c => c.Valor);
                    break;
                case "aprovacao":
                    cotacoes = cotacoes.OrderBy(c => c.Aprovacao);
                    break;
                case "aprovacao_desc":
                    cotacoes = cotacoes.OrderByDescending(c => c.Aprovacao);
                    break;
                default:
                    cotacoes = cotacoes.OrderBy(c => c.Id);
                    break;
            }
            return View(await cotacoes.ToListAsync());
        }

        // GET: Cotacoes/Create
        public IActionResult Create()
        {
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome");
            return View();
        }

        // POST: Cotacoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descricao,FornecedorId,Valor")] Cotacao cotacao)
        {
            // Conversão segura do valor para decimal en-US (ponto como separador)
            if (Request.Form.TryGetValue("Valor", out var valorStr))
            {
                try
                {
                    cotacao.Valor = Convert.ToDecimal(valorStr.ToString(), new System.Globalization.CultureInfo("en-US"));
                }
                catch
                {
                    ModelState.AddModelError("Valor", "Valor inválido. Use o formato 0.00");
                }
            }
            if (ModelState.IsValid)
            {
                cotacao.Aprovacao = 0; // Sempre começa como pendente
                _context.Add(cotacao);
                await _context.SaveChangesAsync();
                TempData["success"] = "Cotação cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome", cotacao.FornecedorId);
            return View(cotacao);
        }

        // GET: Cotacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cotacao = await _context.Cotacoes.FindAsync(id);
            if (cotacao == null) return NotFound();
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome", cotacao.FornecedorId);
            return View(cotacao);
        }

        // POST: Cotacoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,FornecedorId,Valor,Aprovacao")] Cotacao cotacao)
        {
            if (id != cotacao.Id) return NotFound();
            // Se o valor vier com vírgula, converte para ponto antes de salvar
            if (Request.Form.TryGetValue("Valor", out var valorStr))
            {
                var valorConvertido = valorStr.ToString().Replace(',', '.');
                try
                {
                    cotacao.Valor = Convert.ToDecimal(valorConvertido, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    ModelState.AddModelError("Valor", "Valor inválido. Use o formato 0.00");
                }
            }
            if (ModelState.IsValid)
            {
                var cotacaoOriginal = await _context.Cotacoes.FindAsync(id);
                if (cotacaoOriginal == null) return NotFound();
                cotacaoOriginal.Descricao = cotacao.Descricao;
                cotacaoOriginal.FornecedorId = cotacao.FornecedorId;
                cotacaoOriginal.Valor = cotacao.Valor;
                cotacaoOriginal.Aprovacao = cotacao.Aprovacao;
                await _context.SaveChangesAsync();
                TempData["success"] = "Alteração feita com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome", cotacao.FornecedorId);
            return View(cotacao);
        }

        // GET: Cotacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cotacao = await _context.Cotacoes.Include(c => c.Fornecedor).FirstOrDefaultAsync(m => m.Id == id);
            if (cotacao == null) return NotFound();
            return View(cotacao);
        }

        // POST: Cotacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cotacao = await _context.Cotacoes.FindAsync(id);
            if (cotacao != null)
            {
                _context.Cotacoes.Remove(cotacao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Cotacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cotacao = await _context.Cotacoes.Include(c => c.Fornecedor).FirstOrDefaultAsync(m => m.Id == id);
            if (cotacao == null) return NotFound();
            return View(cotacao);
        }
    }
}
