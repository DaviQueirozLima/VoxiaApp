namespace Voxia.Domain.Entities
{
    public class Usuario
    {
        public Guid UsuarioId { get; set; }   // use Guid pra manter o padrão das outras entidades
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Login tradicional
        public string? SenhaHash { get; set; }

        // Login via Google
        public string? GoogleId { get; set; }

        // Outros campos opcionais
        public string? FotoPerfil { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        // Relacionamentos
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
        public ICollection<Reproducao> Reproducoes { get; set; } = new List<Reproducao>();
    }
}
