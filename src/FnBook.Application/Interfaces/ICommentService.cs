using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponseDto>> GetAllAsync();
    Task<CommentResponseDto?> GetByIdAsync(Guid id);
    Task<CommentResponseDto> CreateAsync(CreateCommentDto dto);
    Task<CommentResponseDto?> UpdateAsync(Guid id, UpdateCommentDto dto);
    Task<bool> DeleteAsync(Guid id);
}
