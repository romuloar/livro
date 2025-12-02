using Livro.Domain.Port.Livro.Read.GetLivroById.In;
using Livro.Domain.Port.Livro.Read.GetLivroById.Out;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.Livro.Read.GetLivroById;

public interface IGetLivroByIdPort
{
    Task<ResultDetail<GetLivroByIdOut>> ExecuteAsync(GetLivroByIdIn input);
}
