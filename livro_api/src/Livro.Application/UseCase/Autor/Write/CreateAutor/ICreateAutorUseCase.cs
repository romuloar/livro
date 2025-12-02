using Livro.Domain.Port.Autor.Write.CreateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Write.CreateAutor;

public interface ICreateAutorUseCase
{
    Task<ResultDetail<AutorDomain>> ExecuteAsync(CreateAutorIn input);
}
