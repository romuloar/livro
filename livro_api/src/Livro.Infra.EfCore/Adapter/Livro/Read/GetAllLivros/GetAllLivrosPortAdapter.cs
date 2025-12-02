using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Livro.Read.GetAllLivros;
using Livro.Domain.Entity.Livro;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Livro.Read.GetAllLivros;

public class GetAllLivrosPortAdapter : IGetAllLivrosPort
{
    private readonly AppDbContext _context;

    public GetAllLivrosPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<List<LivroDomain>>> ExecuteAsync()
    {
        try
        {
            var livrosEntity = await _context.Livros
                .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
                .Include(l => l.LivroAssuntos).ThenInclude(la => la.Assunto)
                .Include(l => l.LivroValores).ThenInclude(lv => lv.TipoCompra)
                .ToListAsync();

            var livrosDomain = livrosEntity.Select(l => l.ToDomain()).ToList();
            return livrosDomain.GetResultDetailSuccess("Livros recuperados com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<List<LivroDomain>>();
        }
    }
}
