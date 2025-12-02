using Rom.Annotations;
using Rom.Domain;
using Rom.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Entity.Assunto;

public class AssuntoDomain : RomBaseDomain
{
    public Ulid CodAs { get; set; } = Ulid.NewUlid();
    
    [RequiredString(ErrorMessage = "Descrição do assunto é obrigatória")]
    [StringLength(20, ErrorMessage = "Descrição do assunto deve ter no máximo 20 caracteres")]
    public string Descricao { get; set; } = string.Empty;
}
