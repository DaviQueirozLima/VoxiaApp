using Voxia.Domain.Entities;

namespace Voxia.Domain.Repositories.CardsRepositories;

public interface ICardsRepositories
{
    Task<List<Card>> ObterTodosAsync();
    Task<Card?> ObterPorIdAsync(Guid cardId);
    Task<List<Card>> ObterPorUsuarioAsync(Guid usuarioId); // cards criados por esse usuário
    Task<IEnumerable<Card>> ObterCardsPorCategoriaAsync(Guid categoriaId);
    Task<Card> AdicionarAsync(Card card);
    Task AtualizarAsync(Card card);
    Task RemoverAsync(Card card);
    Task IncrementarCliquesAsync(Card card);
    Task<List<Card>> ObterMaisUsadosAsync(int quantidade = 5);
}
