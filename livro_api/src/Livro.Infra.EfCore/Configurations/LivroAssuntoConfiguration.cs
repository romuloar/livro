using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class LivroAssuntoConfiguration : IEntityTypeConfiguration<LivroAssuntoEntity>
{
    public void Configure(EntityTypeBuilder<LivroAssuntoEntity> builder)
    {
        builder.ToTable("Livro_Assunto");
        
        builder.HasKey(e => new { e.Livro_Codl, e.Assunto_CodAs });
        
        builder.Property(e => e.Livro_Codl)
            .HasColumnName("Livro_Codl")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Assunto_CodAs)
            .HasColumnName("Assunto_CodAs")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
        
        // Relationship: LivroAssunto -> Livro
        builder.HasOne(e => e.Livro)
            .WithMany(l => l.LivroAssuntos)
            .HasForeignKey(e => e.Livro_Codl)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relationship: LivroAssunto -> Assunto
        builder.HasOne(e => e.Assunto)
            .WithMany(a => a.LivroAssuntos)
            .HasForeignKey(e => e.Assunto_CodAs)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
