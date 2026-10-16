using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => c.ToResponseDto());
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category?.ToResponseDto();
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = dto.ToEntity();
        var created = await _categoryRepository.CreateAsync(category);
        return created.ToResponseDto();
    }

    public async Task<CategoryResponseDto?> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            return null;

        category.ApplyUpdate(dto);
        var updated = await _categoryRepository.UpdateAsync(category);
        return updated.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }
}
