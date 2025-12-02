using Livro.Domain.Port.Assunto.Write.DeleteAssunto;
using Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Assunto.Write.DeleteAssunto;

public class DeleteAssuntoUseCase : IDeleteAssuntoUseCase
{
    private readonly IDeleteAssuntoPort _port;

    public DeleteAssuntoUseCase(IDeleteAssuntoPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<bool>> ExecuteAsync(DeleteAssuntoIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<bool>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
