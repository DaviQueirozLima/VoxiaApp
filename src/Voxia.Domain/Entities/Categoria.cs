namespace Voxia.Domain.Entities
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        //Relacionamentos
        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
