using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsOriginsController : ControllerBase
{
    private readonly INewsOriginService _service;

    public NewsOriginsController(INewsOriginService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NewsOriginResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NewsOriginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var origin = await _service.GetByIdAsync(id);
        return origin is null ? NotFound() : Ok(origin);
    }

    [HttpGet("by-news/{newsId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<NewsOriginResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNewsId(Guid newsId) =>
        Ok(await _service.GetByNewsIdAsync(newsId));

    [HttpPost]
    [ProducesResponseType(typeof(NewsOriginResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateNewsOriginDto dto)
    {
        try
        {
            var origin = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = origin.OrigemId }, origin);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(NewsOriginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNewsOriginDto dto)
    {
        try
        {
            var origin = await _service.UpdateAsync(id, dto);
            return origin is null ? NotFound() : Ok(origin);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
