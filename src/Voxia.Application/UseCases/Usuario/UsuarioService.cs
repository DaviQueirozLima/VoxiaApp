using Voxia.Domain.DTOs.Usuario;
using Voxia.Domain.DTOs;
using Voxia.Domain.Entities;
using Voxia.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Voxia.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly string _jwtSecret;

        public UsuarioService(IUsuarioRepository usuarioRepository, string jwtSecret)
        {
            _usuarioRepository = usuarioRepository;
            _jwtSecret = jwtSecret;
        }

        // Retorna todos os usuários
        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nome = u.Nome,
                Email = u.Email,
                FotoPerfil = u.FotoPerfil
            });
        }

        // Retorna usuário por ID
        public async Task<UsuarioDto?> GetByIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                FotoPerfil = usuario.FotoPerfil
            };
        }

        // Cria usuário
        public async Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                FotoPerfil = dto.FotoPerfil,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            await _usuarioRepository.AddAsync(usuario);

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                FotoPerfil = usuario.FotoPerfil
            };
        }

        // Atualiza usuário (apenas nome e foto)
        public async Task<UsuarioDto?> UpdateAsync(Guid id, UsuarioUpdateDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            usuario.Nome = dto.Nome;
            usuario.FotoPerfil = dto.FotoPerfil ?? usuario.FotoPerfil;

            await _usuarioRepository.UpdateAsync(usuario);

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                FotoPerfil = usuario.FotoPerfil
            };
        }

        // Deleta usuário
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }

        // Login com email + senha
        public async Task<JwtTokenDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nome)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: "VoxiaAPI",
                audience: "VoxiaApp",
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtTokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
