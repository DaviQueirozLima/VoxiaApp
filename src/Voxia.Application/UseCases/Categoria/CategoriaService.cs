using Voxia.Domain.DTOs.Cards;
using Voxia.Domain.DTOs.Categoria;
using Voxia.Domain.Entities;
using Voxia.Domain.Repositories.CardsRepositories;
using Voxia.Domain.Repositories.CategoriaRepositories;

namespace Voxia.Application.UseCases;
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ICardsRepositories _cardsRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository, ICardsRepositories cardsRepository)
    {   
        _categoriaRepository = categoriaRepository;
        _cardsRepository = cardsRepository;
    }

    public async Task<IEnumerable<CardDto>> ObterCardsPorCategoriaAsync(Guid categoriaId)
    {
        var cards = await _cardsRepository.ObterCardsPorCategoriaAsync(categoriaId);
        return cards.Select(c => new CardDto
        {
            CardId = c.CardId,
            Nome = c.Nome,
            Imagem = c.Imagem,
            Audio = c.Audio,
            ContagemCliques = c.ContagemCliques,
            CategoriaNome = c.Categoria.Nome ?? string.Empty,
            UsuarioId = c.UsuarioId,
            DataCriacao = c.DataCriacao
        });
    }

    public async Task<IEnumerable<CategoriaDto>> ObterTodasAsync()
    {
        var categorias = await _categoriaRepository.ObterTodasAsync();
        return categorias.Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nome = c.Nome,
            DataCriacao = c.DataCriacao,
        });
    }

    public async Task<CategoriaDto> ObterPorIdAsync(Guid id)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(id);
        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");
        return new CategoriaDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            DataCriacao = categoria.DataCriacao,
        };
    }

}
