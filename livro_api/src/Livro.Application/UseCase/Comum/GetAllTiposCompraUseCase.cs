using Livro.Domain.Port.TipoCompra.GetAllTiposCompra;
using Livro.Domain.Entity.TipoCompra;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Comum;

public class GetAllTiposCompraUseCase : IGetAllTiposCompraUseCase
{
    private readonly IGetAllTiposCompraPort _port;

    public GetAllTiposCompraUseCase(IGetAllTiposCompraPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<List<TipoCompraDomain>>> ExecuteAsync() => await _port.ExecuteAsync();
}
