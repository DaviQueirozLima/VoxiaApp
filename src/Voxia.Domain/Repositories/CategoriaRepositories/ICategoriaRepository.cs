using Voxia.Domain.Entities;

namespace Voxia.Domain.Repositories.CategoriaRepositories;
public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> ObterTodasAsync();
    Task<Categoria?> ObterPorIdAsync(Guid id);
}
