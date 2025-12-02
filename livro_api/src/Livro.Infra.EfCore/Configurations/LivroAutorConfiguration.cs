using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class LivroAutorConfiguration : IEntityTypeConfiguration<LivroAutorEntity>
{
    public void Configure(EntityTypeBuilder<LivroAutorEntity> builder)
    {
        builder.ToTable("Livro_Autor");
        
        builder.HasKey(e => new { e.Livro_Codl, e.Autor_CodAu });
        
        builder.Property(e => e.Livro_Codl)
            .HasColumnName("Livro_Codl")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Autor_CodAu)
            .HasColumnName("Autor_CodAu")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
        
        // Relationship: LivroAutor -> Livro
        builder.HasOne(e => e.Livro)
            .WithMany(l => l.LivroAutores)
            .HasForeignKey(e => e.Livro_Codl)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relationship: LivroAutor -> Autor
        builder.HasOne(e => e.Autor)
            .WithMany(a => a.LivroAutores)
            .HasForeignKey(e => e.Autor_CodAu)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
