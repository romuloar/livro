using Rom.Domain;
using Rom.Domain.Base;
using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;

public class UpdateAssuntoIn : RomBaseDomain
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public Ulid Id { get; set; }
    
    [RequiredString(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(20, ErrorMessage = "Descrição deve ter no máximo 20 caracteres")]
    public string Descricao { get; set; } = string.Empty;
}
