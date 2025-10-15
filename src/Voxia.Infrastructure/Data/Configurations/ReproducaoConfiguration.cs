using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voxia.Domain.Entities;

namespace Voxia.Infrastructure.Data.Configurations
{
    public class ReproducaoConfiguration : IEntityTypeConfiguration<Reproducao>
    {
        public void Configure(EntityTypeBuilder<Reproducao> builder)
        {
            builder.HasKey(r => r.IdReproducao);

            builder.Property(r => r.DataHora)
                .IsRequired();

            // Relacionamento com Usuario
            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Reproducoes)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Card
            builder.HasOne(r => r.Card)
                .WithMany(c => c.Reproducoes)
                .HasForeignKey(r => r.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice opcional para consultas rápidas
            builder.HasIndex(r => new { r.UsuarioId, r.CardId });
        }
    }
}
