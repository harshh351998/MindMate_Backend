using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindMate.Application.DTOs;
using MindMate.Application.Interfaces;

namespace MindMate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterUserAsync(registerDto);
                return CreatedAtAction(nameof(Register), result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginUserDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", details = ex.Message });
            }
        }
        
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Get token from Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            // Client-side handling is sufficient for basic logout, but we could implement token blacklisting here
            // This would require:
            // 1. A BlacklistedToken entity
            // 2. Repository for token management
            // 3. Service to add token to blacklist
            // await _authService.BlacklistTokenAsync(token);
            
            return Ok(new { message = "Logout successful" });
        }
    }
} 