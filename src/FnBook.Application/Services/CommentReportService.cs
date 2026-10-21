using System.ComponentModel.DataAnnotations;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class CommentReportService(ICommentReportRepository repository, ICommentRepository commentRepository, IUserRepository userRepository) : ICommentReportService
{
    public async Task<IEnumerable<CommentReportResponseDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(e => e.ToResponseDto());

    public async Task<CommentReportResponseDto?> GetByIdAsync(Guid id) =>
        (await repository.GetByIdAsync(id))?.ToResponseDto();

    public async Task<IEnumerable<CommentReportResponseDto>> GetByCommentIdAsync(Guid commentId) =>
        (await repository.GetByCommentIdAsync(commentId)).Select(e => e.ToResponseDto());

    public async Task<CommentReportResponseDto> CreateAsync(CreateCommentReportDto dto)
    {
        await ValidateAsync(dto);
        return (await repository.CreateAsync(dto.ToEntity())).ToResponseDto();
    }

    public async Task<CommentReportResponseDto?> UpdateAsync(Guid id, UpdateCommentReportDto dto)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;
        await ValidateAsync(dto);
        entity.ApplyUpdate(dto);
        return (await repository.UpdateAsync(entity)).ToResponseDto();
    }

    public Task<bool> DeleteAsync(Guid id) => repository.DeleteAsync(id);

    private async Task ValidateAsync(CreateCommentReportDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        if (dto.CommentId == Guid.Empty || await commentRepository.GetByIdAsync(dto.CommentId) is null)
            throw new ArgumentException("Comentário não encontrado.");
        if (dto.UserId == Guid.Empty || await userRepository.GetByIdAsync(dto.UserId) is null)
            throw new ArgumentException("Usuário não encontrado.");
    }
}
