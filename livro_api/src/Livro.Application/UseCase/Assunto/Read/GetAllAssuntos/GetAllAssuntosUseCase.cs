using Livro.Domain.Port.Assunto.Read.GetAllAssuntos;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Read.GetAllAssuntos;

public class GetAllAssuntosUseCase : IGetAllAssuntosUseCase
{
    private readonly IGetAllAssuntosPort _port;

    public GetAllAssuntosUseCase(IGetAllAssuntosPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<List<AssuntoDomain>>> ExecuteAsync() => await _port.ExecuteAsync();
}
