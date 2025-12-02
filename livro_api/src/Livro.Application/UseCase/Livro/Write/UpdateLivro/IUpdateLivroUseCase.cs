using Livro.Domain.Port.Livro.Write.UpdateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Livro.Write.UpdateLivro;

public interface IUpdateLivroUseCase
{
    Task<ResultDetail<LivroDomain>> ExecuteAsync(UpdateLivroIn input);
}
