using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Autor.Read.GetAutorById;
using Livro.Domain.Port.Autor.Read.GetAutorById.In;
using Livro.Domain.Entity.Autor;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Autor.Read.GetAutorById;

public class GetAutorByIdPortAdapter : IGetAutorByIdPort
{
    private readonly AppDbContext _context;

    public GetAutorByIdPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<AutorDomain>> ExecuteAsync(GetAutorByIdIn input)
    {
        try
        {
            var autorEntity = await _context.Autores
                .FirstOrDefaultAsync(a => a.CodAu == input.Id);

            if (autorEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<AutorDomain>("Autor não encontrado");

            var autorDomain = autorEntity.ToDomain();
            return autorDomain.GetResultDetailSuccess("Autor recuperado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<AutorDomain>();
        }
    }
}
