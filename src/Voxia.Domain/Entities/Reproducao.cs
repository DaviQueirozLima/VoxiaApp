namespace Voxia.Domain.Entities
{
    public class Reproducao
    {
        public Guid IdReproducao { get; set; }
        public Guid CardId { get; set; }
        public Guid UsuarioId { get; set; }

        public DateTime DataHora { get; set; } = DateTime.UtcNow;

        public Card Card { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
