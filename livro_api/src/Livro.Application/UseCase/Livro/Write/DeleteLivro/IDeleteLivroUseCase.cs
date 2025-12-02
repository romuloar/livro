using Livro.Domain.Port.Livro.Write.DeleteLivro.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Livro.Write.DeleteLivro;

public interface IDeleteLivroUseCase
{
    Task<ResultDetail<bool>> ExecuteAsync(DeleteLivroIn input);
}
