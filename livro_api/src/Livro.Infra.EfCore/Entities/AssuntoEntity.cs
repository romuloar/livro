using Livro.Domain.Entity.Assunto;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de AssuntoDomain e adiciona relacionamentos de navegação.
/// </summary>
public class AssuntoEntity : AssuntoDomain
{
    // Propriedades de navegação do EF Core
    public List<LivroAssuntoEntity> LivroAssuntos { get; set; } = new();
}
