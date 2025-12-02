using Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Assunto.Write.DeleteAssunto;

public interface IDeleteAssuntoPort
{
    Task<ResultDetail<bool>> ExecuteAsync(DeleteAssuntoIn input);
}
