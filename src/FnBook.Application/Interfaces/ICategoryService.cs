using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(Guid id);
    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryResponseDto?> UpdateAsync(Guid id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}
