using Livro.Domain.Port.Livro.Read.GetLivroById;
using Livro.Domain.Port.Livro.Read.GetLivroById.In;
using Livro.Domain.Port.Livro.Read.GetLivroById.Out;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Livro.Read.GetLivroById;

public class GetLivroByIdUseCase : IGetLivroByIdUseCase
{
    private readonly IGetLivroByIdPort _port;

    public GetLivroByIdUseCase(IGetLivroByIdPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<GetLivroByIdOut>> ExecuteAsync(GetLivroByIdIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<GetLivroByIdOut>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
