using Livro.Domain.Port.Assunto.Write.CreateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Write.CreateAssunto;

public interface ICreateAssuntoUseCase
{
    Task<ResultDetail<AssuntoDomain>> ExecuteAsync(CreateAssuntoIn input);
}
