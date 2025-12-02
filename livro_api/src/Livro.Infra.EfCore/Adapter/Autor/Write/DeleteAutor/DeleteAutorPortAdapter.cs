using Livro.Domain.Port.Autor.Write.DeleteAutor;
using Livro.Domain.Port.Autor.Write.DeleteAutor.In;
using Livro.Infra.EfCore.Contexts;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Autor.Write.DeleteAutor;

public class DeleteAutorPortAdapter : IDeleteAutorPort
{
    private readonly AppDbContext _context;

    public DeleteAutorPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<bool>> ExecuteAsync(DeleteAutorIn input)
    {
        try
        {
            var autorEntity = await _context.Autores.FindAsync(input.Id);
            if (autorEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<bool>("Autor não encontrado");

            _context.Autores.Remove(autorEntity);
            await _context.SaveChangesAsync();
            return true.GetResultDetailSuccess("Autor excluído com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<bool>();
        }
    }
}
