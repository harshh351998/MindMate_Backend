using System;
using System.Collections.Generic;
using MindMate.Core.Enums;

namespace MindMate.Application.DTOs
{
    public class JournalEntryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string EntryText { get; set; }
        public int MoodRating { get; set; }
        public SentimentType Sentiment { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsPrivate { get; set; } = false;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Username { get; set; } // Include username for display
    }
    
    public class JournalEntryCreateDto
    {
        public Guid UserId { get; set; }
        public string EntryText { get; set; }
        public int MoodRating { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsPrivate { get; set; } = false;
        public SentimentType? Sentiment { get; set; } // Optional sentiment from client, will be derived from mood if not provided
    }
    
    public class JournalEntryUpdateDto
    {
        public Guid Id { get; set; }
        public string EntryText { get; set; }
        public int MoodRating { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsPrivate { get; set; } = false;
        public SentimentType? Sentiment { get; set; } // Optional sentiment from client, will be derived from mood if not provided
    }
} 