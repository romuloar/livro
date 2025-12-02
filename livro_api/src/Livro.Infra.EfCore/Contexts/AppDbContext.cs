using Microsoft.EntityFrameworkCore;
using Livro.Infra.EfCore.Entities;
using Livro.Domain.Models;

namespace Livro.Infra.EfCore.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<LivroEntity> Livros { get; set; }
    public DbSet<AutorEntity> Autores { get; set; }
    public DbSet<AssuntoEntity> Assuntos { get; set; }
    public DbSet<TipoCompraEntity> TiposCompra { get; set; }
    public DbSet<LivroAutorEntity> LivroAutores { get; set; }
    public DbSet<LivroAssuntoEntity> LivroAssuntos { get; set; }
    public DbSet<LivroValorEntity> LivroValores { get; set; }
    
    // VIEW para relatórios
    public DbSet<RelatorioLivro> RelatorioLivros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações IEntityTypeConfiguration do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        // Configura a VIEW como keyless
        modelBuilder.Entity<RelatorioLivro>()
            .ToView("vw_RelatorioLivros")
            .HasNoKey();
        
        // Seeds são executados via DatabaseSeeder.SeedAsync() no Program.cs
        // Isso garante verificação de existência antes de inserir
    }
}
