using Livro.Domain.Port.Autor.Write.DeleteAutor;
using Livro.Domain.Port.Autor.Write.DeleteAutor.In;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Autor.Write.DeleteAutor;

public class DeleteAutorUseCase : IDeleteAutorUseCase
{
    private readonly IDeleteAutorPort _port;

    public DeleteAutorUseCase(IDeleteAutorPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<bool>> ExecuteAsync(DeleteAutorIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<bool>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
