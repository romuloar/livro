using Livro.Domain.Port.Autor.Read.GetAutorById;
using Livro.Domain.Port.Autor.Read.GetAutorById.In;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Read.GetAutorById;

public class GetAutorByIdUseCase : IGetAutorByIdUseCase
{
    private readonly IGetAutorByIdPort _port;

    public GetAutorByIdUseCase(IGetAutorByIdPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<AutorDomain>> ExecuteAsync(GetAutorByIdIn input) => await _port.ExecuteAsync(input);
}
