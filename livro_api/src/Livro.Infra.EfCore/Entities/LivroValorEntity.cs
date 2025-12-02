using Livro.Domain.Entity.LivroValor;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de LivroValorDomain e adiciona relacionamentos de navegação.
/// </summary>
public class LivroValorEntity : LivroValorDomain
{
    // Propriedades de navegação do EF Core
    public LivroEntity Livro { get; set; } = null!;
    public TipoCompraEntity TipoCompra { get; set; } = null!;
}
