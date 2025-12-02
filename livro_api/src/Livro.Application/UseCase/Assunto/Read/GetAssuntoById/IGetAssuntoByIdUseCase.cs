using Livro.Domain.Port.Assunto.Read.GetAssuntoById.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Read.GetAssuntoById;

public interface IGetAssuntoByIdUseCase
{
    Task<ResultDetail<AssuntoDomain>> ExecuteAsync(GetAssuntoByIdIn input);
}
