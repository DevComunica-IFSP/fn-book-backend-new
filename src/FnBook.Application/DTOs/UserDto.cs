namespace FnBook.Application.DTOs;

public record CreateUserDto
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? ProfilePicture { get; init; }
    public Guid? StateId { get; init; }
    public string UserType { get; init; } = string.Empty;
}

public record UpdateUserDto
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? ProfilePicture { get; init; }
    public Guid? StateId { get; init; }
    public string UserType { get; init; } = string.Empty;
}

public record UserResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? ProfilePicture { get; init; }
    public Guid? StateId { get; init; }
    public string UserType { get; init; } = string.Empty;
    public int ContributorScore { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
