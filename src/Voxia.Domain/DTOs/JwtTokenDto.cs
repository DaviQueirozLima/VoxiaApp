namespace Voxia.Domain.DTOs;
public class JwtTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
