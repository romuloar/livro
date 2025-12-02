using Livro.Domain.Port.Autor.Write.CreateAutor;
using Livro.Domain.Port.Autor.Write.CreateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Autor.Write.CreateAutor;

public class CreateAutorUseCase : ICreateAutorUseCase
{
    private readonly ICreateAutorPort _port;

    public CreateAutorUseCase(ICreateAutorPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<AutorDomain>> ExecuteAsync(CreateAutorIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<AutorDomain>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
