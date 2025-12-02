using Livro.Domain.Port.Livro.Write.UpdateLivro;
using Livro.Domain.Port.Livro.Write.UpdateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Livro.Write.UpdateLivro;

public class UpdateLivroUseCase : IUpdateLivroUseCase
{
    private readonly IUpdateLivroPort _port;

    public UpdateLivroUseCase(IUpdateLivroPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<LivroDomain>> ExecuteAsync(UpdateLivroIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<LivroDomain>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
