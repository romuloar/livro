using Livro.Domain.Port.Livro.Read.GetAllLivros;
using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Livro.Read.GetAllLivros;

public class GetAllLivrosUseCase : IGetAllLivrosUseCase
{
    private readonly IGetAllLivrosPort _port;

    public GetAllLivrosUseCase(IGetAllLivrosPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<List<LivroDomain>>> ExecuteAsync()
    {
        return await _port.ExecuteAsync();
    }
}
