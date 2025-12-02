using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class TipoCompraConfiguration : IEntityTypeConfiguration<TipoCompraEntity>
{
    public void Configure(EntityTypeBuilder<TipoCompraEntity> builder)
    {
        builder.ToTable("TipoCompra");
        
        builder.HasKey(e => e.CodTc);
        
        builder.Property(e => e.CodTc)
            .HasColumnName("CodTc")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Descricao)
            .HasMaxLength(200)
            .IsRequired();
    }
}
