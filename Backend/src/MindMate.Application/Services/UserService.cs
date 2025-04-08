using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindMate.Application.DTOs;
using MindMate.Application.Interfaces;
using MindMate.Core.Entities;
using MindMate.Core.Interfaces;

namespace MindMate.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserWithJournalEntriesDto> GetUserWithJournalEntriesAsync(Guid id)
        {
            var user = await _userRepository.GetUserWithJournalEntriesAsync(id);
            if (user == null)
                return null;

            return new UserWithJournalEntriesDto
            {
                Id = user.Id,
                Username = user.Username,
                JournalEntries = user.JournalEntries.Select(j => new JournalEntryDto
                {
                    Id = j.Id,
                    UserId = j.UserId,
                    EntryText = j.EntryText,
                    MoodRating = j.MoodRating,
                    Sentiment = j.Sentiment,
                    DateCreated = j.DateCreated,
                    DateModified = j.DateModified != default ? j.DateModified : j.DateCreated,
                    Username = user.Username
                })
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            if (await _userRepository.UsernameExistsAsync(userCreateDto.Username))
                throw new InvalidOperationException("Username already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userCreateDto.Username,
                PasswordHash = HashPassword(userCreateDto.Password) // In a real app, use a proper password hashing library
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            // Verify current password
            if (user.PasswordHash != HashPassword(userUpdateDto.CurrentPassword))
                throw new InvalidOperationException("Current password is incorrect");

            // Update properties
            user.Username = userUpdateDto.Username;
            
            // Update password if provided
            if (!string.IsNullOrEmpty(userUpdateDto.NewPassword))
                user.PasswordHash = HashPassword(userUpdateDto.NewPassword);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<UserDto> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return null;

            // Verify password
            if (user.PasswordHash != HashPassword(password))
                return null;

            return MapToDto(user);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        // Simple password hashing for demonstration
        // In a real application, use a proper password hashing library like BCrypt
        public string HashPassword(string password)
        {
            // This is just a placeholder - DO NOT use this in production
            // Use a proper password hashing library instead
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
            );
        }
        
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }
        
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetByIdAsync(changePasswordDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {changePasswordDto.UserId} not found");

            // Verify current password
            if (user.PasswordHash != HashPassword(changePasswordDto.CurrentPassword))
                throw new InvalidOperationException("Current password is incorrect");
                
            // Update password
            user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            
            return true;
        }
    }
} 