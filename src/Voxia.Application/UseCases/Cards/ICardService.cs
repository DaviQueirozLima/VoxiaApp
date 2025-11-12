using Voxia.Domain.DTOs.Cards;

namespace Voxia.Application.UseCases.Cards;

public interface ICardService
{
    Task<List<CardDto>> ObterTodosAsync();
    Task<CardDto?> ObterPorIdAsync(Guid cardId);
    Task<List<CardDto>> ObterPorUsuarioAsync(Guid usuarioId); // parâmetro de volta
    Task<CardDto> AdicionarAsync(CardCreateDto dto);
    Task AtualizarAsync(Guid cardId, CardCreateDto dto);
    Task RemoverAsync(Guid cardId);
    Task IncrementarCliquesAsync(Guid cardId);
    Task<List<CardDto>> ObterMaisUsadosAsync(int quantidade = 5);
}
