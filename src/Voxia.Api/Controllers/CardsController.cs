using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxia.Application.UseCases.Cards;
using Voxia.Domain.DTOs.Cards;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardsController(ICardService cardService)
    {
        _cardService = cardService;
    }

    // Obter todos os cards
    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var cards = await _cardService.ObterTodosAsync();
        return Ok(cards);
    }

    // Obter card por id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(Guid id)
    {
        var card = await _cardService.ObterPorIdAsync(id);
        if (card == null)
            return NotFound(new { mensagem = "Card não encontrado." });

        return Ok(card);
    }

    // Obter cards de um usuário específico
    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> GetPorUsuario(Guid usuarioId)
    {
        var cards = await _cardService.ObterPorUsuarioAsync(usuarioId);
        return Ok(cards);
    }

    // Adicionar um novo card (com imagem e áudio)
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromForm] CardCreateDto dto)
    {
        var card = await _cardService.AdicionarAsync(dto);
        return CreatedAtAction(nameof(GetPorId), new { id = card.CardId }, card);
    }

    // Atualizar um card existente (com possibilidade de alterar imagem e áudio)
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromForm] CardCreateDto dto)
    {
        try
        {
            var cardExistente = await _cardService.ObterPorIdAsync(id);
            if (cardExistente == null)
                return NotFound(new { mensagem = "Card não encontrado." });

            // 🔒 Impedir atualização de cards pré-definidos
            if (cardExistente.UsuarioId == null)
                return StatusCode(403, new { mensagem = "Cards pré-definidos não podem ser alterados." });

            await _cardService.AtualizarAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao atualizar o card.", detalhe = ex.Message });
        }
    }

    // Remover um card
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        try
        {
            var cardExistente = await _cardService.ObterPorIdAsync(id);
            if (cardExistente == null)
                return NotFound(new { mensagem = "Card não encontrado." });

            // 🔒 Impedir exclusão de cards pré-definidos
            if (cardExistente.UsuarioId == null)
                return StatusCode(403, new { mensagem = "Cards pré-definidos não podem ser excluídos." });

            await _cardService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao remover o card.", detalhe = ex.Message });
        }
    }

    // Incrementar cliques
    [HttpPost("{id}/clique")]
    public async Task<IActionResult> IncrementarCliques(Guid id)
    {
        try
        {
            await _cardService.IncrementarCliquesAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    // Obter os cards mais usados
    [HttpGet("mais-usados")]
    public async Task<IActionResult> MaisUsados([FromQuery] int quantidade = 5)
    {
        var cards = await _cardService.ObterMaisUsadosAsync(quantidade);
        return Ok(cards);
    }
}
