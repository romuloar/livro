using Livro.Domain.Entity.Assunto;
using Livro.Domain.Port.Assunto.Read.GetAssuntoById.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Assunto.Read.GetAssuntoById;

public interface IGetAssuntoByIdPort
{
    Task<ResultDetail<AssuntoDomain>> ExecuteAsync(GetAssuntoByIdIn input);
}
