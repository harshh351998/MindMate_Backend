using System;
using System.Collections.Generic;

namespace MindMate.Application.DTOs
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }

    public class UserWithJournalEntriesResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<JournalEntryResponseDto> JournalEntries { get; set; }
    }
} 