using Rom.Annotations;
using Rom.Annotations.Attributes.String;
using Rom.Domain;
using Rom.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Entity.Livro;

public class LivroDomain : RomBaseDomain
{
    public Ulid Codl { get; set; } = Ulid.NewUlid();
    
    [RequiredString(ErrorMessage = "Título é obrigatório")]
    [StringLength(40, ErrorMessage = "Título deve ter no máximo 40 caracteres")]
    public string Titulo { get; set; } = string.Empty;
    
    [RequiredString(ErrorMessage = "Editora é obrigatória")]
    [StringLength(40, ErrorMessage = "Editora deve ter no máximo 40 caracteres")]
    public string Editora { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue, ErrorMessage = "Edição deve ser maior que zero")]
    public int Edicao { get; set; }
    
    [RequiredString(ErrorMessage = "Ano de publicação é obrigatório")]
    [StringLengthEquals(4, ErrorMessage = "Ano de publicação deve ter exatamente 4 caracteres")]
    public string AnoPublicacao { get; set; } = string.Empty;
}
