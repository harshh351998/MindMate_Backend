using System;
using System.Collections.Generic;

namespace MindMate.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        
        // Navigation property for JournalEntries
        public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
    }
} 