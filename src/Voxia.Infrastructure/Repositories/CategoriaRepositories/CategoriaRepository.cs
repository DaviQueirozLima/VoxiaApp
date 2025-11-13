using Microsoft.EntityFrameworkCore;
using Voxia.Domain.Entities;
using Voxia.Domain.Repositories.CategoriaRepositories;
using Voxia.Infrastructure.Data;

namespace Voxia.Infrastructure.Repositories.CategoriaRepositories;
public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;
    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Categoria>> ObterTodasAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .Select(c => new Categoria
            {
                Id = c.Id,
                Nome = c.Nome,
                DataCriacao = c.DataCriacao
            })
            .ToListAsync();
    }
    public async Task<Categoria?> ObterPorIdAsync(Guid id)
    {
        return await _context.Categorias
            .Include(c => c.Cards)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
