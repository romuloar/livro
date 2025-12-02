using Livro.Domain.Entity.Livro;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Entidade específica do EF Core que herda de LivroDomain e adiciona relacionamentos de navegação.
/// Isso mantém o Domain limpo de detalhes de infraestrutura.
/// </summary>
public class LivroEntity : LivroDomain
{
    // Propriedades de navegação do EF Core
    public List<LivroAutorEntity> LivroAutores { get; set; } = new();
    public List<LivroAssuntoEntity> LivroAssuntos { get; set; } = new();
    public List<LivroValorEntity> LivroValores { get; set; } = new();
}
