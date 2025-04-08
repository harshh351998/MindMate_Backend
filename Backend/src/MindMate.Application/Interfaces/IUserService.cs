using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindMate.Application.DTOs;

namespace MindMate.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserWithJournalEntriesDto> GetUserWithJournalEntriesAsync(Guid id);
        Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);
        Task<UserDto> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto);
        Task DeleteUserAsync(Guid id);
        Task<UserDto> AuthenticateAsync(string username, string password);
        string HashPassword(string password);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
} 