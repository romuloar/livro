using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Livro.Read.GetAllLivros;

public interface IGetAllLivrosPort
{
    Task<ResultDetail<List<LivroDomain>>> ExecuteAsync();
}
