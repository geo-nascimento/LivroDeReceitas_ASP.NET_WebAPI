using MeuLivroDeReceitas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepository;

public class LivroDeReceitasContext : DbContext
{
    public LivroDeReceitasContext(DbContextOptions<LivroDeReceitasContext> options):base(options)
    {
        
    }

    public DbSet<Usuario>? Usuarios { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LivroDeReceitasContext).Assembly);
    }
}