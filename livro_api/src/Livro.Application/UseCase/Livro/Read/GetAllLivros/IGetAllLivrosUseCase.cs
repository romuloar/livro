using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Livro.Read.GetAllLivros;

public interface IGetAllLivrosUseCase
{
    Task<ResultDetail<List<LivroDomain>>> ExecuteAsync();
}
