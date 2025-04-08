using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MindMate.Application.Interfaces;
using MindMate.Core.Entities;

namespace MindMate.Infrastructure.JwtGenerator
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string token, DateTime expiration) GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "fallbackSecretKey1234567890abcdefghijklmnopqrstuvwxyz"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            var expirationDays = int.TryParse(_configuration["Jwt:ExpirationDays"], out int days) ? days : 7;
            var expiration = DateTime.UtcNow.AddDays(expirationDays);
            
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "mindmate",
                audience: _configuration["Jwt:Audience"] ?? "mindmate-users",
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );
            
            return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }
    }
} 