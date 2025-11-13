using Voxia.Domain.DTOs.Cards;
using Voxia.Domain.DTOs.Categoria;

namespace Voxia.Application.UseCases;
public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> ObterTodasAsync();
    Task<IEnumerable<CardDto>> ObterCardsPorCategoriaAsync(Guid categoriaId);
    Task<CategoriaDto> ObterPorIdAsync(Guid id);
}
