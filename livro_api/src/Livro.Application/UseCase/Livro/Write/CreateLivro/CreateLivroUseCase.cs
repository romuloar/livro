using Livro.Domain.Port.Livro.Write.CreateLivro;
using Livro.Domain.Port.Livro.Write.CreateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Application.UseCase.Livro.Write.CreateLivro;

public class CreateLivroUseCase : ICreateLivroUseCase
{
    private readonly ICreateLivroPort _port;

    public CreateLivroUseCase(ICreateLivroPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<LivroDomain>> ExecuteAsync(CreateLivroIn input)
    {
        if (!input.IsValidDomain)
        {            
            return await ResultDetailExtensions.GetErrorAsync<LivroDomain>("Parametros inválidos");
        }

        return await _port.ExecuteAsync(input);
    }
}
