using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Assunto.Read.GetAllAssuntos;

public interface IGetAllAssuntosPort
{
    Task<ResultDetail<List<AssuntoDomain>>> ExecuteAsync();
}
