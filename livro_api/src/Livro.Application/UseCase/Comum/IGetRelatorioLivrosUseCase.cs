using Livro.Domain.Entity.Relatorio;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Comum;

public interface IGetRelatorioLivrosUseCase
{
    Task<ResultDetail<List<RelatorioLivroDomain>>> ExecuteAsync();
}
