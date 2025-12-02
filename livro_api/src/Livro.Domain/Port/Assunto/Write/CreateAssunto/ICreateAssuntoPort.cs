using Livro.Domain.Port.Assunto.Write.CreateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Assunto.Write.CreateAssunto;

public interface ICreateAssuntoPort
{
    Task<ResultDetail<AssuntoDomain>> ExecuteAsync(CreateAssuntoIn input);
}
