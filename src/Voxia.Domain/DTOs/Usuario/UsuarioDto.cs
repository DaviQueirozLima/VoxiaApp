namespace Voxia.Domain.DTOs.Usuario
{
    public class UsuarioDto
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
