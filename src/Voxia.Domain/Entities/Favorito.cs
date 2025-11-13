namespace Voxia.Domain.Entities
{
    public class Favorito
    {
        public Guid FavoritoId { get; set; }
        public Guid CardId { get; set; }
        public Guid UsuarioId { get; set; } 

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        //Relacionamentos
        public Usuario Usuario { get; set; } = null!;
        public Card Card { get; set; } = null!;
    }
}
