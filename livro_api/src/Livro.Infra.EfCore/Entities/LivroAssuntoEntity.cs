using Livro.Domain.Entity.LivroAssunto;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de LivroAssuntoDomain e adiciona relacionamentos de navegação.
/// </summary>
public class LivroAssuntoEntity : LivroAssuntoDomain
{
    // Propriedades de navegação do EF Core
    public LivroEntity Livro { get; set; } = null!;
    public AssuntoEntity Assunto { get; set; } = null!;
}
