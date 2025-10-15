using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voxia.Domain.Entities;

namespace Voxia.Infrastructure.Data.Configurations
{
    public class FavoritoConfiguration : IEntityTypeConfiguration<Favorito>
    {
        public void Configure(EntityTypeBuilder<Favorito> builder)
        {
            builder.HasKey(f => f.FavoritoId);

            // Relacionamento com Usuario
            builder.HasOne(f => f.Usuario)
                .WithMany(u => u.Favoritos)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Card
            builder.HasOne(f => f.Card)
                .WithMany(c => c.Favoritos)
                .HasForeignKey(f => f.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único para evitar duplicidade de favorito por usuário
            builder.HasIndex(f => new { f.UsuarioId, f.CardId })
            .IsUnique(); // suficiente, sem HasDatabaseName
        }
    }
}
