using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<LivroEntity>
{
    public void Configure(EntityTypeBuilder<LivroEntity> builder)
    {
        builder.ToTable("Livro");
        
        builder.HasKey(e => e.Codl);
        
        builder.Property(e => e.Codl)
            .HasColumnName("Codl")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Titulo)
            .HasMaxLength(40)
            .IsRequired();
            
        builder.Property(e => e.Editora)
            .HasMaxLength(40);
            
        builder.Property(e => e.Edicao)
            .IsRequired();
            
        builder.Property(e => e.AnoPublicacao)
            .HasMaxLength(4)
            .IsRequired();
    }
}
