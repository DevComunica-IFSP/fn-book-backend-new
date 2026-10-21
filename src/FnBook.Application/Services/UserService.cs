using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;

    public UserService(IUserRepository userRepository, IPasswordHashService passwordHashService)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => u.ToResponseDto());
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToResponseDto();
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Password);

        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new InvalidOperationException("Email já está em uso");

        var user = dto.ToEntity();
        user.PasswordHash = _passwordHashService.Hash(dto.Password);

        var created = await _userRepository.CreateAsync(user);
        return created.ToResponseDto();
    }

    public async Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return null;

        if (await _userRepository.ExistsByEmailAsync(dto.Email, id))
            throw new InvalidOperationException("Email já está em uso");

        user.ApplyUpdate(dto);
        var updated = await _userRepository.UpdateAsync(user);
        return updated.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}
