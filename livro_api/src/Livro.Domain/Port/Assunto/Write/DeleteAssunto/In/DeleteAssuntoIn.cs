using Rom.Domain;
using Rom.Domain.Base;
using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;

public class DeleteAssuntoIn : RomBaseDomain
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public Ulid Id { get; set; }
}
