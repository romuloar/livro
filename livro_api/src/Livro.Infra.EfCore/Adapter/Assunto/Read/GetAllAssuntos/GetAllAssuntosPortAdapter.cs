using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Assunto.Read.GetAllAssuntos;
using Livro.Domain.Entity.Assunto;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Assunto.Read.GetAllAssuntos;

public class GetAllAssuntosPortAdapter : IGetAllAssuntosPort
{
    private readonly AppDbContext _context;

    public GetAllAssuntosPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<List<AssuntoDomain>>> ExecuteAsync()
    {
        try
        {
            var assuntosEntity = await _context.Assuntos.ToListAsync();
            var assuntosDomain = assuntosEntity.Select(a => a.ToDomain()).ToList();
            return assuntosDomain.GetResultDetailSuccess("Assuntos recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<List<AssuntoDomain>>();
        }
    }
}
