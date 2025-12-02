using Livro.Domain.Port.Assunto.Write.UpdateAssunto;
using Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Assunto.Write.UpdateAssunto;

public class UpdateAssuntoUseCase : IUpdateAssuntoUseCase
{
    private readonly IUpdateAssuntoPort _port;

    public UpdateAssuntoUseCase(IUpdateAssuntoPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<AssuntoDomain>> ExecuteAsync(UpdateAssuntoIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
