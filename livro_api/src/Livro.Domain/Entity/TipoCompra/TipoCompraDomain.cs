namespace Livro.Domain.Entity.TipoCompra;

public class TipoCompraDomain
{
    public Ulid CodTc { get; set; } = Ulid.NewUlid();
    public string Descricao { get; set; } = string.Empty;
}
