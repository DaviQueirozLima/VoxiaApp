using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voxia.Domain.Entities;

namespace Voxia.Infrastructure.Data.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(c => c.CardId);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Imagem)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.Audio)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            // Relacionamento com Categoria
            builder.HasOne(c => c.Categoria)
                .WithMany(cat => cat.Cards)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Favoritos
            builder.HasMany(c => c.Favoritos)
                .WithOne(f => f.Card)
                .HasForeignKey(f => f.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Reproducoes
            builder.HasMany(c => c.Reproducoes)
                .WithOne(r => r.Card)
                .HasForeignKey(r => r.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
