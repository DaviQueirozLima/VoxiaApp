using Microsoft.AspNetCore.Http;

namespace Voxia.Domain.DTOs.Cards
{
    public class CardUpdateDto
    {
        public string? Nome { get; set; }

        // Categoria opcional para atualizar
        public Guid? CategoriaId { get; set; }

        // Imagem opcional para atualizar
        public IFormFile? Imagem { get; set; }

        // Áudio opcional para atualizar
        public IFormFile? Audio { get; set; }
    }
}
