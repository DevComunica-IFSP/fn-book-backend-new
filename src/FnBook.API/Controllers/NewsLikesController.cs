using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/news-likes")]
public class NewsLikesController : ControllerBase
{
    private readonly INewsLikeService _newsLikeService;

    public NewsLikesController(INewsLikeService newsLikeService)
    {
        _newsLikeService = newsLikeService;
    }

    [HttpGet("noticia/{newsId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<NewsLikeResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNews(Guid newsId)
    {
        var likes = await _newsLikeService.GetByNewsIdAsync(newsId);
        return Ok(likes);
    }

    [HttpPost]
    [ProducesResponseType(typeof(NewsLikeResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateNewsLikeDto dto)
    {
        var like = await _newsLikeService.CreateAsync(dto);
        if (like is null)
            return Conflict(new { error = "Usuário já curtiu essa notícia" });

        return CreatedAtAction(nameof(GetByNews), new { newsId = like.NewsId }, like);
    }

    [HttpDelete("{newsId:guid}/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid newsId, Guid userId)
    {
        var deleted = await _newsLikeService.DeleteAsync(userId, newsId);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
