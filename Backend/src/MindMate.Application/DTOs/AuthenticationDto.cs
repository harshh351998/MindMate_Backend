using System;
using System.ComponentModel.DataAnnotations;

namespace MindMate.Application.DTOs
{
    public class LoginRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string NewPassword { get; set; }
    }
} 