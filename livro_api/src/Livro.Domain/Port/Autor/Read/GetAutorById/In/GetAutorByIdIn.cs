using Rom.Domain;
using Rom.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Autor.Read.GetAutorById.In;

public class GetAutorByIdIn : RomBaseDomain
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public Ulid Id { get; set; }
}
