using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class LivroValorConfiguration : IEntityTypeConfiguration<LivroValorEntity>
{
    public void Configure(EntityTypeBuilder<LivroValorEntity> builder)
    {
        builder.ToTable("Livro_Valor");
        
        builder.HasKey(e => e.Codlv);
        
        builder.Property(e => e.Codlv)
            .HasColumnName("Codlv")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Livro_Codl)
            .HasColumnName("Livro_Codl")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.TipoCompra_CodTc)
            .HasColumnName("TipoCompra_CodTc")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Valor)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        // Relationship: LivroValor -> Livro
        builder.HasOne(e => e.Livro)
            .WithMany(l => l.LivroValores)
            .HasForeignKey(e => e.Livro_Codl)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relationship: LivroValor -> TipoCompra
        builder.HasOne(e => e.TipoCompra)
            .WithMany(t => t.LivroValores)
            .HasForeignKey(e => e.TipoCompra_CodTc)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
