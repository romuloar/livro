using Livro.Domain.Port.Livro.Write.UpdateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Livro.Write.UpdateLivro;

public interface IUpdateLivroPort
{
    Task<ResultDetail<LivroDomain>> ExecuteAsync(UpdateLivroIn input);
}
