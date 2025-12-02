using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Relatorio.GetRelatorioLivros;
using Livro.Domain.Entity.Relatorio;
using Livro.Infra.EfCore.Contexts;
using Rom.Result.Domain;
using Rom.Result.Extensions;
using Livro.Domain.Models;

namespace Livro.Infra.EfCore.Adapter.Relatorio.Read.GetRelatorioLivros;

public class GetRelatorioLivrosPortAdapter : IGetRelatorioLivrosPort
{
    private readonly AppDbContext _context;

    public GetRelatorioLivrosPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<List<RelatorioLivroDomain>>> ExecuteAsync()
    {
        try
        {
            // Busca dados da VIEW
            var viewData = await _context.RelatorioLivros.ToListAsync();

            // Agrupa por autor e livro para estruturar conforme domain
            var relatorio = viewData
                .GroupBy(v => new { v.CodAu, v.NomeAutor })
                .SelectMany(autorGroup =>
                    autorGroup
                        .Where(v => !string.IsNullOrEmpty(v.Codl))
                        .GroupBy(v => v.Codl)
                        .Select(livroGroup =>
                        {
                            var primeiro = livroGroup.First();
                            return new RelatorioLivroDomain
                            {
                                AutorNome = primeiro.NomeAutor,
                                LivroTitulo = primeiro.Titulo ?? string.Empty,
                                Editora = primeiro.Editora ?? string.Empty,
                                Edicao = primeiro.Edicao ?? 0,
                                AnoPublicacao = primeiro.AnoPublicacao ?? string.Empty,
                                Assuntos = !string.IsNullOrEmpty(primeiro.Assuntos)
                                    ? primeiro.Assuntos.Split(", ").Distinct().ToList()
                                    : new List<string>(),
                                Valores = primeiro.QuantidadeTiposCompra > 0
                                    ? new List<ValorLivroDomain>
                                    {
                                        new() { TipoCompra = "Total", Valor = primeiro.ValorTotal }
                                    }
                                    : new List<ValorLivroDomain>()
                            };
                        })
                )
                .OrderBy(r => r.AutorNome)
                .ThenBy(r => r.LivroTitulo)
                .ToList();

            return relatorio.GetResultDetailSuccess("Relatório gerado com sucesso a partir da VIEW");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<List<RelatorioLivroDomain>>();
        }
    }
}
