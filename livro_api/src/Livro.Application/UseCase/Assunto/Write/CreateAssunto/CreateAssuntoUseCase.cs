using Livro.Domain.Port.Assunto.Write.CreateAssunto;
using Livro.Domain.Port.Assunto.Write.CreateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Assunto.Write.CreateAssunto;

public class CreateAssuntoUseCase : ICreateAssuntoUseCase
{
    private readonly ICreateAssuntoPort _port;

    public CreateAssuntoUseCase(ICreateAssuntoPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<AssuntoDomain>> ExecuteAsync(CreateAssuntoIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>("Parametros inv�lidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
