using Livro.Domain.Entity.Autor;
using Livro.Domain.Port.Autor.Read.GetAutorById.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Autor.Read.GetAutorById;

public interface IGetAutorByIdPort
{
    Task<ResultDetail<AutorDomain>> ExecuteAsync(GetAutorByIdIn input);
}
