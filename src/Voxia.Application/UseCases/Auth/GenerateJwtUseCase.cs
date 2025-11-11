using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Voxia.Domain.DTOs;
using Voxia.Domain.Entities;

namespace Voxia.Application.UseCases.Auth
{
    public class GenerateJwtUseCase : IGenerateJwtUseCase
    {
        private readonly string _jwtSecret;

        public GenerateJwtUseCase(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public JwtTokenDto Execute(Usuario usuario)
        {
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
       expires: expiration,
       signingCredentials: creds,
       claims: claims
   );

            return new JwtTokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
