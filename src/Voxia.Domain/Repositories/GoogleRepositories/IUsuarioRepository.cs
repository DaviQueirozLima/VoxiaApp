using Voxia.Domain.Entities;

namespace Voxia.Domain.Repositories.GoogleRepositories
{
    public interface IUsuarioRepository
    {
        Task <Usuario?> GetByGoogleAsync(string googleId);
        Task<Usuario> AddAsync(Usuario usuario);
        Task SaveChangesAsync();
    }
}
