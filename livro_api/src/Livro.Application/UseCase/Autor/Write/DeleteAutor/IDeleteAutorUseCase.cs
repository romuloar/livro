using Livro.Domain.Port.Autor.Write.DeleteAutor.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Write.DeleteAutor;

public interface IDeleteAutorUseCase
{
    Task<ResultDetail<bool>> ExecuteAsync(DeleteAutorIn input);
}
