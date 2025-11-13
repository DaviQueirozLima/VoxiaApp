namespace Voxia.Domain.DTOs.Cards
{
    public class CardDto
    {
        public Guid CardId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Imagem { get; set; } = string.Empty; // Caminho ou URL
        public string Audio { get; set; } = string.Empty;  // Caminho ou URL
        public int ContagemCliques { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }

        public Guid? UsuarioId { get; set; } // Nullable, pois nem todos os cards terão usuário
        public DateTime DataCriacao { get; set; }
    }
}
