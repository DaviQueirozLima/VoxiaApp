using Microsoft.AspNetCore.Mvc;
using Voxia.Application.UseCases;

namespace Voxia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        try
        {
            var categorias = await _categoriaService.ObterTodasAsync();
            return Ok(categorias);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet("{categoriaId}/cards")]
    public async Task<IActionResult> ObterCardsPorCategoria(Guid id)
    {
        try
        {
            var cards = await _categoriaService.ObterCardsPorCategoriaAsync(id);
            return Ok(cards);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet("{categoriaId}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            return Ok(categoria);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
