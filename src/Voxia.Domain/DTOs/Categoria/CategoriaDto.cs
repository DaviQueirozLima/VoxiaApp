namespace Voxia.Domain.DTOs.Categoria;
public class CategoriaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
