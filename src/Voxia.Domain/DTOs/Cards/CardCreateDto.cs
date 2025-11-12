using Microsoft.AspNetCore.Http;

namespace Voxia.Domain.DTOs.Cards
{
    public class CardCreateDto
    {
        public string Nome { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }

        // Arquivos enviados pelo Swagger ou pelo frontend
        public IFormFile Imagem { get; set; } = null!;
        public IFormFile Audio { get; set; } = null!;
    }
}
