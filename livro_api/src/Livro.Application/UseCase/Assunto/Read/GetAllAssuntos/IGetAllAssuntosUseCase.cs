using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Read.GetAllAssuntos;

public interface IGetAllAssuntosUseCase
{
    Task<ResultDetail<List<AssuntoDomain>>> ExecuteAsync();
}
