using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Assunto.Read.GetAssuntoById;
using Livro.Domain.Port.Assunto.Read.GetAssuntoById.In;
using Livro.Domain.Entity.Assunto;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Assunto.Read.GetAssuntoById;

public class GetAssuntoByIdPortAdapter : IGetAssuntoByIdPort
{
    private readonly AppDbContext _context;

    public GetAssuntoByIdPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<AssuntoDomain>> ExecuteAsync(GetAssuntoByIdIn input)
    {
        try
        {
            var assuntoEntity = await _context.Assuntos
                .FirstOrDefaultAsync(a => a.CodAs == input.Id);

            if (assuntoEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>("Assunto não encontrado");

            var assuntoDomain = assuntoEntity.ToDomain();
            return assuntoDomain.GetResultDetailSuccess("Assunto recuperado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<AssuntoDomain>();
        }
    }
}
