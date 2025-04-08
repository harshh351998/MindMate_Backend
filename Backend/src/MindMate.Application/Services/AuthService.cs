using System;
using System.Threading.Tasks;
using MindMate.Application.DTOs;
using MindMate.Application.Interfaces;
using MindMate.Core.Entities;
using MindMate.Core.Interfaces;

namespace MindMate.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserService _userService;

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IUserService userService)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userService = userService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto)
        {
            // Get the user by username
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Verify the password
            if (user.PasswordHash != _userService.HashPassword(loginDto.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Generate JWT token
            var (token, expiration) = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = expiration,
                UserId = user.Id,
                Username = user.Username
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            // Get the user by username
            var user = await _userRepository.GetByUsernameAsync(loginRequest.Username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Verify the password
            if (user.PasswordHash != _userService.HashPassword(loginRequest.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Generate JWT token
            var (token, expiration) = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = expiration,
                UserId = user.Id,
                Username = user.Username
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
        {
            // Check if username already exists
            if (await _userRepository.UsernameExistsAsync(registerRequest.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Create a new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = registerRequest.Username,
                PasswordHash = _userService.HashPassword(registerRequest.Password)
            };

            // Save the user
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Generate JWT token
            var (token, expiration) = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = expiration,
                UserId = user.Id,
                Username = user.Username
            };
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            // Check if username already exists
            if (await _userRepository.UsernameExistsAsync(registerDto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Create a new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                PasswordHash = _userService.HashPassword(registerDto.Password)
            };

            // Save the user
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
} 