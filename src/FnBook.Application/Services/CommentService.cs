using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<CommentResponseDto>> GetAllAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
        return comments.Select(c => c.ToResponseDto());
    }

    public async Task<CommentResponseDto?> GetByIdAsync(Guid id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        return comment?.ToResponseDto();
    }

    public async Task<CommentResponseDto> CreateAsync(CreateCommentDto dto)
    {
        var comment = dto.ToEntity();
        var created = await _commentRepository.CreateAsync(comment);
        return created.ToResponseDto();
    }

    public async Task<CommentResponseDto?> UpdateAsync(Guid id, UpdateCommentDto dto)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment is null)
            return null;

        comment.ApplyUpdate(dto);
        var updated = await _commentRepository.UpdateAsync(comment);
        return updated.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _commentRepository.DeleteAsync(id);
    }
}
