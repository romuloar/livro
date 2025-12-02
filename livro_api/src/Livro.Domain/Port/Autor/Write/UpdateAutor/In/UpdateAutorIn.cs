using Rom.Domain;
using Rom.Domain.Base;
using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Port.Autor.Write.UpdateAutor.In;

public class UpdateAutorIn : RomBaseDomain
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public Ulid Id { get; set; }
    
    [RequiredString(ErrorMessage = "Nome é obrigatório")]
    [StringLength(40, ErrorMessage = "Nome deve ter no máximo 40 caracteres")]
    public string Nome { get; set; } = string.Empty;
}
