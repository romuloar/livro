using Livro.Domain.Port.Autor.Write.DeleteAutor.In;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Autor.Write.DeleteAutor;

public interface IDeleteAutorPort
{
    Task<ResultDetail<bool>> ExecuteAsync(DeleteAutorIn input);
}
