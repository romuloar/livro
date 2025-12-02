namespace Livro.Domain.Entity.LivroValor;

public class LivroValorDomain
{
    public Ulid Codlv { get; set; } = Ulid.NewUlid();
    public Ulid Livro_Codl { get; set; }
    public Ulid TipoCompra_CodTc { get; set; }
    public decimal Valor { get; set; }
}
