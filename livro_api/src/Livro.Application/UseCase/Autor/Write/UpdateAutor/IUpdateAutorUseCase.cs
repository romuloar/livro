using Livro.Domain.Port.Autor.Write.UpdateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Write.UpdateAutor;

public interface IUpdateAutorUseCase
{
    Task<ResultDetail<AutorDomain>> ExecuteAsync(UpdateAutorIn input);
}
