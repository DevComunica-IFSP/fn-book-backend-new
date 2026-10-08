using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface IUfService
{
    Task<IEnumerable<UfResponseDto>> GetAllAsync();
    Task<UfResponseDto?> GetByIdAsync(Guid id);
    Task<UfResponseDto> CreateAsync(CreateUfDto dto);
    Task<UfResponseDto?> UpdateAsync(Guid id, UpdateUfDto dto);
    Task<bool> DeleteAsync(Guid id);
}
