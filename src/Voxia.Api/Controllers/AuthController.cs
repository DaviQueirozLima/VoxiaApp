using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voxia.Application.UseCases.Auth;
using Voxia.Domain.Entities;

namespace Voxia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IGoogleLoginUseCase _googleLoginUseCase;
        private readonly IGenerateJwtUseCase _generateJwtUseCase;

        public AuthController(IGoogleLoginUseCase googleLoginUseCase, IGenerateJwtUseCase generateJwtUseCase)
        {
            _googleLoginUseCase = googleLoginUseCase;
            _generateJwtUseCase = generateJwtUseCase;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                Usuario usuario = await _googleLoginUseCase.ExecuteAsync(request.IdToken);
                var jwt = _generateJwtUseCase.Execute(usuario);

                return Ok(new
                {
                    user = new
                    {
                        usuario.UsuarioId,
                        usuario.Nome,
                        usuario.Email,
                        usuario.FotoPerfil
                    },
                    token = jwt.Token,
                    expiration = jwt.Expiration
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { message = $"Autenticado com sucesso! ID: {userId}" });
        }

    }

    public class GoogleLoginRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
    

}



