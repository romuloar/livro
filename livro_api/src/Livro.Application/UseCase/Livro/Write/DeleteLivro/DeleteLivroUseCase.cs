using Livro.Domain.Port.Livro.Write.DeleteLivro;
using Livro.Domain.Port.Livro.Write.DeleteLivro.In;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Livro.Write.DeleteLivro;

public class DeleteLivroUseCase : IDeleteLivroUseCase
{
    private readonly IDeleteLivroPort _port;

    public DeleteLivroUseCase(IDeleteLivroPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<bool>> ExecuteAsync(DeleteLivroIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<bool>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
