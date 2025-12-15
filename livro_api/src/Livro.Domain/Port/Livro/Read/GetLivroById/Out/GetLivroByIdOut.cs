using Livro.Domain.Entity.Autor;
using Livro.Domain.Entity.Assunto;

namespace Livro.Domain.Port.Livro.Read.GetLivroById.Out;

public class GetLivroByIdOut
{
    public Ulid Codl { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Editora { get; set; } = string.Empty;
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; } = string.Empty;
    public List<AutorDomain> ListAutor { get; set; } = new();
    public List<AssuntoDomain> ListAssunto { get; set; } = new();
    public List<LivroValorOut> ListLivroValor { get; set; } = new();
}
