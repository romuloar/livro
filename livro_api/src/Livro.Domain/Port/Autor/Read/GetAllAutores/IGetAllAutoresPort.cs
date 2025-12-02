using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Autor.Read.GetAllAutores;

public interface IGetAllAutoresPort
{
    Task<ResultDetail<List<AutorDomain>>> ExecuteAsync();
}
