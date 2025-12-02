using Livro.Domain.Port.Livro.Write.CreateLivro;
using Livro.Domain.Port.Livro.Write.CreateLivro.In;
using Livro.Domain.Entity.Livro;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Livro.Write.CreateLivro;

public class CreateLivroPortAdapter : ICreateLivroPort
{
    private readonly AppDbContext _context;

    public CreateLivroPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<LivroDomain>> ExecuteAsync(CreateLivroIn input)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var livroEntity = new LivroEntity
            {
                Titulo = input.Titulo,
                Editora = input.Editora,
                Edicao = input.Edicao,
                AnoPublicacao = input.AnoPublicacao
            };

            if (!livroEntity.IsValidDomain)
            {
                var errors = string.Join(", ", livroEntity.ListValidationError.Select(e => e.Message));
                return await ResultDetailExtensions.GetErrorAsync<LivroDomain>(errors);
            }
            
            _context.Livros.Add(livroEntity);
            
            // Adicionar autores
            foreach (var autorId in input.AutoresIds)
            {
                _context.LivroAutores.Add(new LivroAutorEntity
                {
                    Livro_Codl = livroEntity.Codl,
                    Autor_CodAu = autorId
                });
            }

            // Adicionar assuntos
            foreach (var assuntoId in input.AssuntosIds)
            {
                _context.LivroAssuntos.Add(new LivroAssuntoEntity
                {
                    Livro_Codl = livroEntity.Codl,
                    Assunto_CodAs = assuntoId
                });
            }

            // Adicionar valores
            foreach (var valor in input.Valores)
            {
                _context.LivroValores.Add(new LivroValorEntity
                {
                    Livro_Codl = livroEntity.Codl,
                    TipoCompra_CodTc = valor.TipoCompraId,
                    Valor = valor.Valor
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return livroEntity.ToDomain().GetResultDetailSuccess("Livro criado com sucesso");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return await ex.GetResultDetailExceptionAsync<LivroDomain>();
        }
    }
}
