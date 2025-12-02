using Livro.Domain.Port.Assunto.Write.UpdateAssunto;
using Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Assunto.Write.UpdateAssunto;

public class UpdateAssuntoPortAdapter : IUpdateAssuntoPort
{
    private readonly AppDbContext _context;

    public UpdateAssuntoPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<AssuntoDomain>> ExecuteAsync(UpdateAssuntoIn input)
    {
        try
        {
            var assuntoEntity = await _context.Assuntos.FindAsync(input.Id);
            if (assuntoEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>("Assunto não encontrado");

            assuntoEntity.Descricao = input.Descricao;
            
            if (!assuntoEntity.IsValidDomain)
            {
                var errors = string.Join(", ", assuntoEntity.ListValidationError.Select(e => e.Message));
                return await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>(errors);
            }
            
            await _context.SaveChangesAsync();
            
            return assuntoEntity.ToDomain().GetResultDetailSuccess("Assunto atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<AssuntoDomain>();
        }
    }
}
