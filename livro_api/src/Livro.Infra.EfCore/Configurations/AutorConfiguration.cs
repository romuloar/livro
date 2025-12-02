using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class AutorConfiguration : IEntityTypeConfiguration<AutorEntity>
{
    public void Configure(EntityTypeBuilder<AutorEntity> builder)
    {
        builder.ToTable("Autor");
        
        builder.HasKey(e => e.CodAu);
        
        builder.Property(e => e.CodAu)
            .HasColumnName("CodAu")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Nome)
            .HasMaxLength(40)
            .IsRequired();
    }
}
