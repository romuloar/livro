using Livro.Infra.EfCore.Contexts;

namespace Livro.Infra.EfCore.Seeds;

/// <summary>
/// Interface base para todas as classes de Seed.
/// Garante verificação antes de inserir dados.
/// </summary>
public interface ISeed
{
    /// <summary>
    /// Ordem de execução (menor executa primeiro).
    /// Use para controlar dependências de FK: TipoCompra = 1, Livro = 2, LivroValor = 3, etc.
    /// </summary>
    int Order { get; }
    
    /// <summary>
    /// Executa o seed, verificando se os dados já existem antes de inserir.
    /// </summary>
    Task SeedAsync(AppDbContext context);
}
