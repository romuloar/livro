using Livro.Domain.Port.Livro.Write.DeleteLivro;
using Livro.Domain.Port.Livro.Write.DeleteLivro.In;
using Livro.Infra.EfCore.Contexts;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Livro.Write.DeleteLivro;

public class DeleteLivroPortAdapter : IDeleteLivroPort
{
    private readonly AppDbContext _context;

    public DeleteLivroPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<bool>> ExecuteAsync(DeleteLivroIn input)
    {
        try
        {
            var livroEntity = await _context.Livros.FindAsync(input.Id);
            
            if (livroEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<bool>("Livro não encontrado");

            _context.Livros.Remove(livroEntity);
            await _context.SaveChangesAsync();

            return true.GetResultDetailSuccess("Livro excluído com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<bool>();
        }
    }
}
