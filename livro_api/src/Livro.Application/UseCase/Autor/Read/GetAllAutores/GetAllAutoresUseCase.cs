using Livro.Domain.Port.Autor.Read.GetAllAutores;
using Livro.Domain.Entity.Autor;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Autor.Read.GetAllAutores;

public class GetAllAutoresUseCase : IGetAllAutoresUseCase
{
    private readonly IGetAllAutoresPort _port;

    public GetAllAutoresUseCase(IGetAllAutoresPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<List<AutorDomain>>> ExecuteAsync() => await _port.ExecuteAsync();
}
