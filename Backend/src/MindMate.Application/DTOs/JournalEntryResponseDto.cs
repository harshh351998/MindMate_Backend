using System;
using MindMate.Core.Enums;

namespace MindMate.Application.DTOs
{
    public class JournalEntryResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string EntryText { get; set; }
        public int MoodRating { get; set; }
        public SentimentType Sentiment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Username { get; set; }
    }
} 