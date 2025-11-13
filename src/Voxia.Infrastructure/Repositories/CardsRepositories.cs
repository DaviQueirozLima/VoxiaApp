using Microsoft.EntityFrameworkCore;
using Voxia.Domain.Entities;
using Voxia.Domain.Repositories.CardsRepositories;
using Voxia.Infrastructure.Data;

namespace Voxia.Infrastructure.Repositories;

public class CardsRepositories : ICardsRepositories
{
    private readonly AppDbContext _context;

    public CardsRepositories(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Card>> ObterTodosAsync()
    {
        return await _context.Cards
            .Include(c => c.Categoria)
            .ToListAsync();
    }

    public async Task<Card?> ObterPorIdAsync(Guid cardId)
    {
        return await _context.Cards
            .Include(c => c.Categoria)
            .FirstOrDefaultAsync(c => c.CardId == cardId);
    }

    public async Task<List<Card>> ObterPorUsuarioAsync(Guid usuarioId)
    {
        return await _context.Cards
            .Where(c => c.UsuarioId == usuarioId)
            .Include(c => c.Categoria)
            .ToListAsync();
    }

    public async Task<IEnumerable<Card>> ObterCardsPorCategoriaAsync(Guid categoriaId)
    {
        return await _context.Cards
            .AsNoTracking()
            .Where(c => c.CategoriaId == categoriaId)
            .Include(c => c.Categoria)
            .ToListAsync();
    }

    public async Task<Card> AdicionarAsync(Card card)
    {
        _context.Cards.Add(card);
        await _context.SaveChangesAsync();
        return card;
    }

    public async Task AtualizarAsync(Card card)
    {
        _context.Cards.Update(card);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Card card)
    {
        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
    }

    public async Task IncrementarCliquesAsync(Card card)
    {
        card.ContagemCliques++; // agora usa a propriedade correta
        _context.Cards.Update(card);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Card>> ObterMaisUsadosAsync(int quantidade = 5)
    {
        return await _context.Cards
            .OrderByDescending(c => c.ContagemCliques) // propriedade correta
            .Take(quantidade)
            .ToListAsync();
    }
}
