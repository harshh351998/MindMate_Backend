using System;
using System.Collections.Generic;

namespace MindMate.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        
        // We don't include PasswordHash in DTO for security
    }
    
    public class UserCreateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    
    public class UserWithJournalEntriesDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<JournalEntryDto> JournalEntries { get; set; }
    }
} 