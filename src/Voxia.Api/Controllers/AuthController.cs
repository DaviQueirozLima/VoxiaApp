using Microsoft.AspNetCore.Mvc;
using Voxia.Application.UseCases.Auth;
using Voxia.Domain.Entities;

namespace Voxia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IGoogleLoginUseCase _googleLoginUseCase;

        public AuthController(IGoogleLoginUseCase googleLoginUseCase)
        {
            _googleLoginUseCase = googleLoginUseCase;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                Usuario usuario = await _googleLoginUseCase.ExecuteAsync(request.IdToken);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class GoogleLoginRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
}



