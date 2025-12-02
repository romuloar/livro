using Livro.Domain.Entity.TipoCompra;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de TipoCompraDomain e adiciona relacionamentos de navegação.
/// </summary>
public class TipoCompraEntity : TipoCompraDomain
{
    // Propriedades de navegação do EF Core
    public List<LivroValorEntity> LivroValores { get; set; } = new();
}
