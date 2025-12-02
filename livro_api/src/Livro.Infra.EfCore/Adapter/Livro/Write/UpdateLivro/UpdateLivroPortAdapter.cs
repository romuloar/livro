using Microsoft.EntityFrameworkCore;
using Livro.Domain.Port.Livro.Write.UpdateLivro;
using Livro.Domain.Port.Livro.Write.UpdateLivro.In;
using Livro.Domain.Entity.Livro;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Rom.Result.Domain;
using Rom.Result.Extensions;

namespace Livro.Infra.EfCore.Adapter.Livro.Write.UpdateLivro;

public class UpdateLivroPortAdapter : IUpdateLivroPort
{
    private readonly AppDbContext _context;

    public UpdateLivroPortAdapter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultDetail<LivroDomain>> ExecuteAsync(UpdateLivroIn input)
    {
        try
        {
            var livroEntity = await _context.Livros
                .Include(l => l.LivroAutores)
                .Include(l => l.LivroAssuntos)
                .Include(l => l.LivroValores)
                .FirstOrDefaultAsync(l => l.Codl == input.Id);

            if (livroEntity == null)
                return await ResultDetailExtensions.GetErrorAsync<LivroDomain>("Livro não encontrado");

            livroEntity.Titulo = input.Titulo;
            livroEntity.Editora = input.Editora;
            livroEntity.Edicao = input.Edicao;
            livroEntity.AnoPublicacao = input.AnoPublicacao;

            if (!livroEntity.IsValidDomain)
            {
                var errors = string.Join(", ", livroEntity.ListValidationError.Select(e => e.Message));
                return await ResultDetailExtensions.GetErrorAsync<LivroDomain>(errors);
            }

            // Remover relacionamentos existentes
            _context.LivroAutores.RemoveRange(livroEntity.LivroAutores);
            _context.LivroAssuntos.RemoveRange(livroEntity.LivroAssuntos);
            _context.LivroValores.RemoveRange(livroEntity.LivroValores);

            // Adicionar novos relacionamentos
            foreach (var autorId in input.AutoresIds)
            {
                _context.LivroAutores.Add(new LivroAutorEntity
                {
                    Livro_Codl = livroEntity.Codl,
                    Autor_CodAu = autorId
                });
            }

            foreach (var assuntoId in input.AssuntosIds)
            {
                _context.LivroAssuntos.Add(new LivroAssuntoEntity
                {
                    Livro_Codl = livroEntity.Codl,
                    Assunto_CodAs = assuntoId
                });
            }

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
            return livroEntity.ToDomain().GetResultDetailSuccess("Livro atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return await ex.GetResultDetailExceptionAsync<LivroDomain>();
        }
    }
}
