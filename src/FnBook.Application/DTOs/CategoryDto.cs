namespace FnBook.Application.DTOs;

public record CreateCategoryDto
{
    public string Name { get; init; } = string.Empty;
}

public record UpdateCategoryDto
{
    public string Name { get; init; } = string.Empty;
}

public record CategoryResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
