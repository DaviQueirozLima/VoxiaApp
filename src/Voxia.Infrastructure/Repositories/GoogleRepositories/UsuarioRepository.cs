using Microsoft.EntityFrameworkCore;
using Voxia.Domain.Entities;
using Voxia.Domain.Repositories.GoogleRepositories;
using Voxia.Infrastructure.Data;

namespace Voxia.Infrastructure.Repositories.GoogleRepositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        // Busca usuário pelo GoogleId
        public async Task<Usuario?> GetByGoogleAsync(string googleId)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }

        // Adiciona usuário novo no banco
        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            var entry = await _context.Usuarios.AddAsync(usuario);
            return entry.Entity;
        }

        // Salva alterações no banco
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
