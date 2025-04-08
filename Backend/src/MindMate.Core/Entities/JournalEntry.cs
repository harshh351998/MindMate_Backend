using System;
using System.Collections.Generic;
using MindMate.Core.Enums;

namespace MindMate.Core.Entities
{
    public class JournalEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string EntryText { get; set; }
        public int MoodRating { get; set; } // 1 to 5
        public SentimentType Sentiment { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsPrivate { get; set; } = false;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        
        // Navigation property
        public User? User { get; set; }
    }
} 