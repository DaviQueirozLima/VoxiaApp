using Voxia.Application.Services;
using Voxia.Domain.Entities;
using Voxia.Domain.Repositories.GoogleRepositories;

namespace Voxia.Application.UseCases.Auth
{
    public class GoogleLoginUseCase : IGoogleLoginUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly GoogleAuthService _googleAuthService;

        public GoogleLoginUseCase(IUsuarioRepository usuarioRepository, GoogleAuthService googleAuthService)
        {
            _usuarioRepository = usuarioRepository;
            _googleAuthService = googleAuthService;
        }

        public async Task<Usuario> ExecuteAsync(string idToken)
        {
            // 1. Valida o token do Google
            var payload = await _googleAuthService.ValidateTokenAsync(idToken);
            if (payload == null)
                throw new Exception("Token inválido");

            // 2. Busca usuário no banco pelo GoogleId
            var usuario = await _usuarioRepository.GetByGoogleAsync(payload.Subject);

            // 3. Se não existir, cria novo usuário
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    GoogleId = payload.Subject,
                    Nome = payload.Name,
                    Email = payload.Email,
                    FotoPerfil = payload.Picture
                };

                await _usuarioRepository.AddAsync(usuario);
                await _usuarioRepository.SaveChangesAsync();
            }

            // 4. Retorna o usuário
            return usuario;
        }
    }
}
