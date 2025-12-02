using Rom.Annotations;
using Rom.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Livro.Domain.Entity.Autor;

public class AutorDomain : RomBaseDomain
{
    public Ulid CodAu { get; set; } = Ulid.NewUlid();
    
    [RequiredString(ErrorMessage = "Nome do autor é obrigatório")]
    [StringLength(40, ErrorMessage = "Nome do autor deve ter no máximo 40 caracteres")]
    public string Nome { get; set; } = string.Empty;
}
