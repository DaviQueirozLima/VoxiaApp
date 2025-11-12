namespace Voxia.Domain.Entities
{
    public class Card
    {
        public Guid CardId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Imagem { get; set; } = string.Empty;
        public string Audio { get; set; } = string.Empty;
        public int ContagemCliques { get; set; } = 0;

        public Guid CategoriaId { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        // Relacionamentos
        public Guid? UsuarioId { get; set; }  // nullable, só terá valor para cards criados por usuário
        public Usuario? Usuario { get; set; } // nullable também

        public Categoria Categoria { get; set; } = null!;
        public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
        public ICollection<Reproducao> Reproducoes { get; set; } = new List<Reproducao>();
    }
}
