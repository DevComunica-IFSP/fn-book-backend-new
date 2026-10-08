using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/comment-likes")]
public class CommentLikesController : ControllerBase
{
    private readonly ICommentLikeService _commentLikeService;

    public CommentLikesController(ICommentLikeService commentLikeService)
    {
        _commentLikeService = commentLikeService;
    }

    [HttpGet("comentario/{commentId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CommentLikeResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByComment(Guid commentId)
    {
        var likes = await _commentLikeService.GetByCommentIdAsync(commentId);
        return Ok(likes);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommentLikeResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateCommentLikeDto dto)
    {
        var like = await _commentLikeService.CreateAsync(dto);
        if (like is null)
            return Conflict(new { error = "Usuário já curtiu esse comentário" });

        return CreatedAtAction(nameof(GetByComment), new { commentId = like.CommentId }, like);
    }

    [HttpDelete("{commentId:guid}/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid commentId, Guid userId)
    {
        var deleted = await _commentLikeService.DeleteAsync(userId, commentId);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
