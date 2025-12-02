using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Livro.Infra.EfCore.Entities;

namespace Livro.Infra.EfCore.Configurations;

public class AssuntoConfiguration : IEntityTypeConfiguration<AssuntoEntity>
{
    public void Configure(EntityTypeBuilder<AssuntoEntity> builder)
    {
        builder.ToTable("Assunto");
        
        builder.HasKey(e => e.CodAs);
        
        builder.Property(e => e.CodAs)
            .HasColumnName("CodAs")
            .HasConversion(
                ulid => ulid.ToString(),
                value => Ulid.Parse(value))
            .HasMaxLength(26)
            .IsRequired();
            
        builder.Property(e => e.Descricao)
            .HasMaxLength(20)
            .IsRequired();
    }
}
