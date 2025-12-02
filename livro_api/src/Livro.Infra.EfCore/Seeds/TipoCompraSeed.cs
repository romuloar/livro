using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Livro.Infra.EfCore.Seeds;

public class TipoCompraSeed : ISeed
{
    public int Order => 1; // Executa primeiro (não tem FK)
    
    public async Task SeedAsync(AppDbContext context)
    {
        // Verifica se já existem dados na tabela
        if (await context.TiposCompra.AnyAsync())
        {
            return; // Já existe dados, não insere novamente
        }

        // Insere os dados iniciais
        var tiposCompra = new[]
        {
            new TipoCompraEntity { CodTc = Ulid.NewUlid(), Descricao = "Balcão" },
            new TipoCompraEntity { CodTc = Ulid.NewUlid(), Descricao = "Self-Service" },
            new TipoCompraEntity { CodTc = Ulid.NewUlid(), Descricao = "Internet" },
            new TipoCompraEntity { CodTc = Ulid.NewUlid(), Descricao = "Evento" }
        };

        await context.TiposCompra.AddRangeAsync(tiposCompra);
        await context.SaveChangesAsync();
    }
}
