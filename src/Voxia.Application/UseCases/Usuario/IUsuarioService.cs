using Voxia.Domain.DTOs;
using Voxia.Domain.DTOs.Usuario;

namespace Voxia.Application.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto);
        Task<UsuarioDto?> UpdateAsync(Guid id, UsuarioUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);

        // Login
        Task<JwtTokenDto?> LoginAsync(LoginDto dto);
    }
}
