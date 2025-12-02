using Rom.Domain;
using Rom.Annotations;
using Rom.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Assunto.Write.CreateAssunto.In;

public class CreateAssuntoIn : RomBaseDomain
{
    [RequiredString(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(20, ErrorMessage = "Descrição deve ter no máximo 20 caracteres")]
    public string Descricao { get; set; } = string.Empty;
}
