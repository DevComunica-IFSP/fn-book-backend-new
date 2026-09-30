using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NewsResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var news = await _newsService.GetAllAsync();
        return Ok(news);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NewsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var news = await _newsService.GetByIdAsync(id);
        if (news is null)
            return NotFound();

        return Ok(news);
    }

    [HttpPost]
    [ProducesResponseType(typeof(NewsResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateNewsDto dto)
    {
        var news = await _newsService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = news.Id }, news);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(NewsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNewsDto dto)
    {
        var news = await _newsService.UpdateAsync(id, dto);
        if (news is null)
            return NotFound();

        return Ok(news);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _newsService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
