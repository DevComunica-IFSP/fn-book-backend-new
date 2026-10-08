using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class CategoryMapping
{
    public static CategoryResponseDto ToResponseDto(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };

    public static Category ToEntity(this CreateCategoryDto dto) => new()
    {
        Id = Guid.NewGuid(),
        Name = dto.Name
    };

    public static void ApplyUpdate(this Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
    }
}
