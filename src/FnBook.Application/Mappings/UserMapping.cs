using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class UserMapping
{
    public static UserResponseDto ToResponseDto(this User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        ProfilePicture = user.ProfilePicture,
        StateId = user.StateId,
        UserType = user.UserType,
        ContributorScore = user.ContributorScore,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };

    public static User ToEntity(this CreateUserDto dto) => new()
    {
        Id = Guid.NewGuid(),
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = string.Empty,
        ProfilePicture = dto.ProfilePicture,
        StateId = dto.StateId,
        UserType = dto.UserType,
        CreatedAt = DateTime.UtcNow
    };

    public static void ApplyUpdate(this User user, UpdateUserDto dto)
    {
        user.Name = dto.Name;
        user.Email = dto.Email;
        user.ProfilePicture = dto.ProfilePicture;
        user.StateId = dto.StateId;
        user.UserType = dto.UserType;
        user.UpdatedAt = DateTime.UtcNow;
    }
}
