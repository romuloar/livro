using Rom.Domain;
using Rom.Domain.Base;
using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Livro.Write.CreateLivro.In;

public class CreateLivroIn : RomBaseDomain
{
    [RequiredString(ErrorMessage = "Título é obrigatório")]
    [StringLength(40, ErrorMessage = "Título deve ter no máximo 40 caracteres")]
    public string Titulo { get; set; } = string.Empty;
    
    [RequiredString(ErrorMessage = "Editora é obrigatória")]
    [StringLength(40, ErrorMessage = "Editora deve ter no máximo 40 caracteres")]
    public string Editora { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue, ErrorMessage = "Edição deve ser maior que zero")]
    public int Edicao { get; set; }
    
    [RequiredString(ErrorMessage = "Ano de publicação é obrigatório")]
    [StringLength(4, ErrorMessage = "Ano de publicação deve ter 4 caracteres")]
    public string AnoPublicacao { get; set; } = string.Empty;
    
    [RequiredList(ErrorMessage = "Deve ter pelo menos um autor")]
    [ListCountMin(1, ErrorMessage = "Deve ter pelo menos um autor")]
    public List<Ulid> AutoresIds { get; set; } = new();
    
    [RequiredList(ErrorMessage = "Deve ter pelo menos um assunto")]
    [ListCountMin(1, ErrorMessage = "Deve ter pelo menos um assunto")]
    public List<Ulid> AssuntosIds { get; set; } = new();
    
    [RequiredList(ErrorMessage = "Deve ter pelo menos um valor")]
    [ListCountMin(1, ErrorMessage = "Deve ter pelo menos um valor")]
    public List<LivroValorIn> Valores { get; set; } = new();
}

public class LivroValorIn : RomBaseDomain
{
    [Required(ErrorMessage = "TipoCompraId é obrigatório")]
    public Ulid TipoCompraId { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }
}
