using Livro.Domain.Port.Assunto.Write.CreateAssunto;
using Livro.Domain.Port.Assunto.Write.CreateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Assunto.Write.CreateAssunto;

public class CreateAssuntoPortAdapter : ICreateAssuntoPort
{
    private readonly AppDbContext _context;

    public CreateAssuntoPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<AssuntoDomain>> ExecuteAsync(CreateAssuntoIn input)
    {
        try
        {
            var assuntoEntity = new AssuntoEntity { Descricao = input.Descricao };
            
            if (!assuntoEntity.IsValidDomain)
            {
                var errors = string.Join(", ", assuntoEntity.ListValidationError.Select(e => e.Message));
                return await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>(errors);
            }
            
            _context.Assuntos.Add(assuntoEntity);
            await _context.SaveChangesAsync();
            
            return assuntoEntity.ToDomain().GetResultDetailSuccess("Assunto criado com sucesso");
        }
        catch (Exception ex)
        {            
            return await ex.GetResultDetailExceptionAsync<AssuntoDomain>();
        }
    }
}
