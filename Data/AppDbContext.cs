using Microsoft.EntityFrameworkCore;
using ProjetoCSharpWeb.Models;

namespace ProjetoCSharpWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cotacao> Cotacoes { get; set; }
    }
}
