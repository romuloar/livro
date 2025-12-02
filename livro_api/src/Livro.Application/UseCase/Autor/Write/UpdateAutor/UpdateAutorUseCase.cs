using Livro.Domain.Port.Autor.Write.UpdateAutor;
using Livro.Domain.Port.Autor.Write.UpdateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Autor.Write.UpdateAutor;

public class UpdateAutorUseCase : IUpdateAutorUseCase
{
    private readonly IUpdateAutorPort _port;

    public UpdateAutorUseCase(IUpdateAutorPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<AutorDomain>> ExecuteAsync(UpdateAutorIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<AutorDomain>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
