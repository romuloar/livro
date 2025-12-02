using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Livro.Read.GetLivroById;
using Livro.Domain.Port.Livro.Read.GetLivroById.In;
using Livro.Domain.Port.Livro.Read.GetLivroById.Out;
using Livro.Infra.EfCore.Contexts;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Livro.Read.GetLivroById;

public class GetLivroByIdPortAdapter : IGetLivroByIdPort
{
    private readonly AppDbContext _context;

    public GetLivroByIdPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<GetLivroByIdOut>> ExecuteAsync(GetLivroByIdIn input)
    {
        try
        {
            var livroEntity = await _context.Livros
                .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
                .Include(l => l.LivroAssuntos).ThenInclude(la => la.Assunto)
                .Include(l => l.LivroValores).ThenInclude(lv => lv.TipoCompra)
                .FirstOrDefaultAsync(l => l.Codl == input.Id);

            if (livroEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<GetLivroByIdOut>("Livro não encontrado");

            var output = new GetLivroByIdOut
            {
                Codl = livroEntity.Codl,
                Titulo = livroEntity.Titulo,
                Editora = livroEntity.Editora,
                Edicao = livroEntity.Edicao,
                AnoPublicacao = livroEntity.AnoPublicacao,
                ListAutor = livroEntity.LivroAutores
                    .Select(la => new Domain.Entity.Autor.AutorDomain
                    {
                        CodAu = la.Autor.CodAu,
                        Nome = la.Autor.Nome
                    })
                    .ToList(),
                ListAssunto = livroEntity.LivroAssuntos
                    .Select(la => new Domain.Entity.Assunto.AssuntoDomain
                    {
                        CodAs = la.Assunto.CodAs,
                        Descricao = la.Assunto.Descricao
                    })
                    .ToList(),
                ListLivroValor = livroEntity.LivroValores
                    .Select(lv => new LivroValorOut
                    {
                        TipoCompraId = lv.TipoCompra_CodTc,
                        TipoCompraDescricao = lv.TipoCompra.Descricao,
                        Valor = lv.Valor
                    })
                    .ToList()
            };

            return output.GetResultDetailSuccess("Livro recuperado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<GetLivroByIdOut>();
        }
    }
}
