using Livro.Domain.Port.Livro.Read.GetLivroById.In;
using Livro.Domain.Port.Livro.Read.GetLivroById.Out;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Livro.Read.GetLivroById;

public interface IGetLivroByIdUseCase
{
    Task<ResultDetail<GetLivroByIdOut>> ExecuteAsync(GetLivroByIdIn input);
}
