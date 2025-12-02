using Rom.Domain;
using Rom.Domain.Base;
using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Autor.Write.DeleteAutor.In;

public class DeleteAutorIn : RomBaseDomain
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public Ulid Id { get; set; }
}
