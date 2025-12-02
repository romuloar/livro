using Livro.Domain.Entity.LivroAutor;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de LivroAutorDomain e adiciona relacionamentos de navegação.
/// </summary>
public class LivroAutorEntity : LivroAutorDomain
{
    // Propriedades de navegação do EF Core
    public LivroEntity Livro { get; set; } = null!;
    public AutorEntity Autor { get; set; } = null!;
}
