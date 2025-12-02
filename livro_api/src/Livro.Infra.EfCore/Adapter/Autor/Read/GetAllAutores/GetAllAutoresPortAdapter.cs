using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Autor.Read.GetAllAutores;
using Livro.Domain.Entity.Autor;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Autor.Read.GetAllAutores;

public class GetAllAutoresPortAdapter : IGetAllAutoresPort
{
    private readonly AppDbContext _context;

    public GetAllAutoresPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<List<AutorDomain>>> ExecuteAsync()
    {
        try
        {
            var autoresEntity = await _context.Autores.ToListAsync();
            var autoresDomain = autoresEntity.Select(a => a.ToDomain()).ToList();
            return autoresDomain.GetResultDetailSuccess("Autores recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<List<AutorDomain>>();
        }
    }
}
