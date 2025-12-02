namespace Livro.Domain.Entity.Relatorio;

public class RelatorioLivroDomain
{
    public string AutorNome { get; set; } = string.Empty;
    public string LivroTitulo { get; set; } = string.Empty;
    public string Editora { get; set; } = string.Empty;
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; } = string.Empty;
    public List<string> Assuntos { get; set; } = new();
    public List<ValorLivroDomain> Valores { get; set; } = new();
}

public class ValorLivroDomain
{
    public string TipoCompra { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
