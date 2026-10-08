using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class CommentLikeService : ICommentLikeService
{
    private readonly ICommentLikeRepository _commentLikeRepository;

    public CommentLikeService(ICommentLikeRepository commentLikeRepository)
    {
        _commentLikeRepository = commentLikeRepository;
    }

    public async Task<IEnumerable<CommentLikeResponseDto>> GetByCommentIdAsync(Guid commentId)
    {
        var likes = await _commentLikeRepository.GetByCommentIdAsync(commentId);
        return likes.Select(l => l.ToResponseDto());
    }

    public async Task<CommentLikeResponseDto?> CreateAsync(CreateCommentLikeDto dto)
    {
        if (await _commentLikeRepository.ExistsAsync(dto.UserId, dto.CommentId))
            return null;

        var like = dto.ToEntity();
        var created = await _commentLikeRepository.CreateAsync(like);
        return created.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid commentId)
    {
        return await _commentLikeRepository.DeleteAsync(userId, commentId);
    }
}
