using System.Threading.Tasks;
using MindMate.Application.DTOs;

namespace MindMate.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
        Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto);
    }
} 