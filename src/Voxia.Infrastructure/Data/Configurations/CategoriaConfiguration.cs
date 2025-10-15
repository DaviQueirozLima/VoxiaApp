using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voxia.Domain.Entities;

namespace Voxia.Infrastructure.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            // Relacionamento com Card
            builder.HasMany(c => c.Cards)
                .WithOne(ca => ca.Categoria)
                .HasForeignKey(ca => ca.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
