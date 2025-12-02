using Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Write.UpdateAssunto;

public interface IUpdateAssuntoUseCase
{
    Task<ResultDetail<AssuntoDomain>> ExecuteAsync(UpdateAssuntoIn input);
}
