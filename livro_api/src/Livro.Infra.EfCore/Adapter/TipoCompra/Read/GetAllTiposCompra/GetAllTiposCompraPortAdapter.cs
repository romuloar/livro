using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.TipoCompra.GetAllTiposCompra;
using Livro.Domain.Entity.TipoCompra;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.TipoCompra.Read.GetAllTiposCompra;

public class GetAllTiposCompraPortAdapter : IGetAllTiposCompraPort
{
    private readonly AppDbContext _context;

    public GetAllTiposCompraPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<List<TipoCompraDomain>>> ExecuteAsync()
    {
        try
        {
            var tiposEntity = await _context.TiposCompra.ToListAsync();
            var tiposDomain = tiposEntity.Select(t => t.ToDomain()).ToList();
            return await tiposDomain.GetResultDetailSuccessAsync("Tipos de compra recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<List<TipoCompraDomain>>();
        }
    }
}
