using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voxia.Domain.Entities;

namespace Voxia.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.UsuarioId);

            builder.Property(u => u.GoogleId)
                .IsRequired();

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.FotoPerfil)
                .HasMaxLength(300);

            builder.Property(u => u.DataCriacao)
                .IsRequired();

            // Relacionamentos
            builder.HasMany(u => u.Favoritos)
                .WithOne(f => f.Usuario)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Reproducoes)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
