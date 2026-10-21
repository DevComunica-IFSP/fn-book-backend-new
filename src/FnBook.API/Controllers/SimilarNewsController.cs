using System.ComponentModel.DataAnnotations;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimilarNewsController(ISimilarNewsService service) : ControllerBase
{
    [HttpGet("by-news/{newsId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<SimilarNewsResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNewsId(Guid newsId) => Ok(await service.GetByNewsIdAsync(newsId));

    [HttpGet("{newsId:guid}/{similarNewsId:guid}")]
    [ProducesResponseType(typeof(SimilarNewsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid newsId, Guid similarNewsId)
    {
        var entity = await service.GetByIdAsync(newsId, similarNewsId);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SimilarNewsResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSimilarNewsDto dto)
    {
        try
        {
            var entity = await service.CreateAsync(dto);
            return entity is null
                ? Conflict(new { error = "Relação entre notícias já cadastrada." })
                : CreatedAtAction(nameof(GetById), new { entity.NewsId, entity.SimilarNewsId }, entity);
        }
        catch (Exception ex) when (ex is ArgumentException or ValidationException)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{newsId:guid}/{similarNewsId:guid}")]
    [ProducesResponseType(typeof(SimilarNewsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid newsId, Guid similarNewsId, [FromBody] UpdateSimilarNewsDto dto)
    {
        try
        {
            var entity = await service.UpdateAsync(newsId, similarNewsId, dto);
            return entity is null ? NotFound() : Ok(entity);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{newsId:guid}/{similarNewsId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid newsId, Guid similarNewsId) =>
        await service.DeleteAsync(newsId, similarNewsId) ? NoContent() : NotFound();
}
