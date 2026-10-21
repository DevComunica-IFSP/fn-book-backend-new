using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface ICommentReportService
{
    Task<IEnumerable<CommentReportResponseDto>> GetAllAsync();
    Task<CommentReportResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CommentReportResponseDto>> GetByCommentIdAsync(Guid commentId);
    Task<CommentReportResponseDto> CreateAsync(CreateCommentReportDto dto);
    Task<CommentReportResponseDto?> UpdateAsync(Guid id, UpdateCommentReportDto dto);
    Task<bool> DeleteAsync(Guid id);
}
