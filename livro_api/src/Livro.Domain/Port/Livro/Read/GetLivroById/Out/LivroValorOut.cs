namespace Livro.Domain.Port.Livro.Read.GetLivroById.Out;

public class LivroValorOut
{
    public Ulid TipoCompraId { get; set; }
    public string TipoCompraDescricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
