using Livro.Domain.Entity.TipoCompra;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Domain.Port.TipoCompra.GetAllTiposCompra;

public interface IGetAllTiposCompraPort
{
    Task<ResultDetail<List<TipoCompraDomain>>> ExecuteAsync();
}
