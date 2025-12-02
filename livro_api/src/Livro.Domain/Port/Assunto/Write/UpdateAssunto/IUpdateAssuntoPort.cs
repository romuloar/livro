using Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Assunto.Write.UpdateAssunto;

public interface IUpdateAssuntoPort
{
    Task<ResultDetail<AssuntoDomain>> ExecuteAsync(UpdateAssuntoIn input);
}
