using Livro.Domain.Port.Livro.Write.DeleteLivro.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Livro.Write.DeleteLivro;

public interface IDeleteLivroPort
{
    Task<ResultDetail<bool>> ExecuteAsync(DeleteLivroIn input);
}
