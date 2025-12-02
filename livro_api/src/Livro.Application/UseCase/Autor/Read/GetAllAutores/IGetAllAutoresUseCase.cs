using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Read.GetAllAutores;

public interface IGetAllAutoresUseCase
{
    Task<ResultDetail<List<AutorDomain>>> ExecuteAsync();
}
