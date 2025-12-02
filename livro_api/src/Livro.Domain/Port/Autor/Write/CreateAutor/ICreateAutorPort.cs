using Livro.Domain.Port.Autor.Write.CreateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Autor.Write.CreateAutor;

public interface ICreateAutorPort
{
    Task<ResultDetail<AutorDomain>> ExecuteAsync(CreateAutorIn input);
}
