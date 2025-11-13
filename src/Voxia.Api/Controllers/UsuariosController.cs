using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxia.Application.Services;
using Voxia.Domain.DTOs.Usuario;

namespace Voxia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuario
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto)
        {
            // Cria o usuário
            var usuario = await _usuarioService.CreateAsync(dto);

            // Faz login automático para gerar o token
            var loginDto = new LoginDto
            {
                Email = dto.Email,
                Senha = dto.Senha
            };
            var token = await _usuarioService.LoginAsync(loginDto);

            // Retorna dados do usuário + token
            return CreatedAtAction(nameof(GetById), new { id = usuario.UsuarioId }, new
            {
                usuario.UsuarioId,
                usuario.Nome,
                usuario.Email,
                usuario.FotoPerfil,
                Token = token?.Token,
                Expiration = token?.Expiration
            });
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UsuarioUpdateDto dto)
        {
            var usuario = await _usuarioService.UpdateAsync(id, dto);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // DELETE: api/usuario/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _usuarioService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // POST: api/usuario/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _usuarioService.LoginAsync(dto);
            if (token == null) return Unauthorized(new { message = "Email ou senha inválidos" });

            return Ok(token);
        }
    }
}
