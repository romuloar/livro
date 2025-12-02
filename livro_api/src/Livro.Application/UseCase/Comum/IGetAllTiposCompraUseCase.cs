using Livro.Domain.Entity.TipoCompra;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Comum;

public interface IGetAllTiposCompraUseCase
{
    Task<ResultDetail<List<TipoCompraDomain>>> ExecuteAsync();
}
