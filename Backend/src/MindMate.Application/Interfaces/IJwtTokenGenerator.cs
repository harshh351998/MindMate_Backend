using System;
using MindMate.Core.Entities;

namespace MindMate.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string token, DateTime expiration) GenerateToken(User user);
    }
} 