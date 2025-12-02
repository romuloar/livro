namespace Livro.Domain.Models;

public class RelatorioLivro
{
    public string CodAu { get; set; } = string.Empty;
    public string NomeAutor { get; set; } = string.Empty;
    public string? Codl { get; set; }
    public string? Titulo { get; set; }
    public string? Editora { get; set; }
    public int? Edicao { get; set; }
    public string? AnoPublicacao { get; set; }
    public string? Assuntos { get; set; }
    public int QuantidadeTiposCompra { get; set; }
    public decimal ValorTotal { get; set; }
}
