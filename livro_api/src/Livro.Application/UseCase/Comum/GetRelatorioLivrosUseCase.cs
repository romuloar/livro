using Livro.Domain.Port.Relatorio.GetRelatorioLivros;
using Livro.Domain.Entity.Relatorio;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Comum;

public class GetRelatorioLivrosUseCase : IGetRelatorioLivrosUseCase
{
    private readonly IGetRelatorioLivrosPort _port;

    public GetRelatorioLivrosUseCase(IGetRelatorioLivrosPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<List<RelatorioLivroDomain>>> ExecuteAsync() => await _port.ExecuteAsync();
}
