using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface IConsultedSourceService
{
    Task<IEnumerable<ConsultedSourceResponseDto>> GetAllAsync();
    Task<ConsultedSourceResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ConsultedSourceResponseDto>> GetByNewsIdAsync(Guid newsId);
    Task<ConsultedSourceResponseDto> CreateAsync(CreateConsultedSourceDto dto);
    Task<ConsultedSourceResponseDto?> UpdateAsync(Guid id, UpdateConsultedSourceDto dto);
    Task<bool> DeleteAsync(Guid id);
}
