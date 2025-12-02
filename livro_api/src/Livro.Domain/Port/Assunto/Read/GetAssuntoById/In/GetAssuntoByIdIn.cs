using Rom.Domain;
using Rom.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Assunto.Read.GetAssuntoById.In;

public class GetAssuntoByIdIn : RomBaseDomain
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public Ulid Id { get; set; }
}
