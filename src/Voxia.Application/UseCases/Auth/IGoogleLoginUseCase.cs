using Voxia.Domain.Entities;

namespace Voxia.Application.UseCases.Auth
{
    public interface IGoogleLoginUseCase
    {
        Task<Usuario> ExecuteAsync(string idToken);
    }
}
