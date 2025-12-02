using Livro.Domain.Entity.Autor;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de AutorDomain e adiciona relacionamentos de navegação.
/// </summary>
public class AutorEntity : AutorDomain
{
    // Propriedades de navegação do EF Core
    public List<LivroAutorEntity> LivroAutores { get; set; } = new();
}
