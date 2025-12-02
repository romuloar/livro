using Livro.Domain.Port.Livro.Write.CreateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Livro.Write.CreateLivro;

public interface ICreateLivroUseCase
{
    Task<ResultDetail<LivroDomain>> ExecuteAsync(CreateLivroIn input);
}
