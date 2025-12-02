using Livro.Domain.Port.Assunto.Write.DeleteAssunto;
using Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;
using Livro.Infra.EfCore.Contexts;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Assunto.Write.DeleteAssunto;

public class DeleteAssuntoPortAdapter : IDeleteAssuntoPort
{
    private readonly AppDbContext _context;

    public DeleteAssuntoPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<bool>> ExecuteAsync(DeleteAssuntoIn input)
    {
        try
        {
            var assuntoEntity = await _context.Assuntos.FindAsync(input.Id);
            if (assuntoEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<bool>("Assunto não encontrado");

            _context.Assuntos.Remove(assuntoEntity);
            await _context.SaveChangesAsync();
            
            return true.GetResultDetailSuccess("Assunto excluído com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<bool>();
        }
    }
}
