using Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Write.DeleteAssunto;

public interface IDeleteAssuntoUseCase
{
    Task<ResultDetail<bool>> ExecuteAsync(DeleteAssuntoIn input);
}
