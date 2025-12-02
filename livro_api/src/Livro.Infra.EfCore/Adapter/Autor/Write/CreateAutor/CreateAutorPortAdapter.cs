using Livro.Domain.Port.Autor.Write.CreateAutor;
using Livro.Domain.Port.Autor.Write.CreateAutor.In;
using Livro.Domain.Entity.Autor;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Autor.Write.CreateAutor;

public class CreateAutorPortAdapter : ICreateAutorPort
{
    private readonly AppDbContext _context;

    public CreateAutorPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<AutorDomain>> ExecuteAsync(CreateAutorIn input)
    {
        try
        {
            var autorEntity = new AutorEntity { Nome = input.Nome };
            
            if (!autorEntity.IsValidDomain)
            {
                var errors = string.Join(", ", autorEntity.ListValidationError.Select(e => e.Message));
                return await ResultDetailExtensions.GetErrorAsync<AutorDomain>(errors);
            }
                
            _context.Autores.Add(autorEntity);
            await _context.SaveChangesAsync();
            return autorEntity.ToDomain().GetResultDetailSuccess("Autor criado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<AutorDomain>();
        }
    }
}
