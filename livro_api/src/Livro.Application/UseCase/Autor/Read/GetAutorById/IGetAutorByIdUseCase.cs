using Livro.Domain.Port.Autor.Read.GetAutorById.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Read.GetAutorById;

public interface IGetAutorByIdUseCase
{
    Task<ResultDetail<AutorDomain>> ExecuteAsync(GetAutorByIdIn input);
}
