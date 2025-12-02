using Livro.Domain.Port.Autor.Write.UpdateAutor;
using Livro.Domain.Port.Autor.Write.UpdateAutor.In;
using Livro.Domain.Entity.Autor;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Autor.Write.UpdateAutor;

public class UpdateAutorPortAdapter : IUpdateAutorPort
{
    private readonly AppDbContext _context;

    public UpdateAutorPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<AutorDomain>> ExecuteAsync(UpdateAutorIn input)
    {
        try
        {
            var autorEntity = await _context.Autores.FindAsync(input.Id);
            if (autorEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<AutorDomain>("Autor não encontrado");

            autorEntity.Nome = input.Nome;
            
            if (!autorEntity.IsValidDomain)
            {
                var errors = string.Join(", ", autorEntity.ListValidationError.Select(e => e.Message));
                return await ResultDetailExtensions.GetErrorAsync<AutorDomain>(errors);
            }
                
            await _context.SaveChangesAsync();
            return autorEntity.ToDomain().GetResultDetailSuccess("Autor atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<AutorDomain>();
        }
    }
}
