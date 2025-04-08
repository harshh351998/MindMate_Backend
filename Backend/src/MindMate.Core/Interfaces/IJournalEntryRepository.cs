using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindMate.Core.Entities;
using MindMate.Core.Enums;

namespace MindMate.Core.Interfaces
{
    public interface IJournalEntryRepository : IRepository<JournalEntry>
    {
        Task<IEnumerable<JournalEntry>> GetJournalEntriesByUserIdAsync(Guid userId);
        Task<IEnumerable<JournalEntry>> GetJournalEntriesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<JournalEntry>> GetJournalEntriesBySentimentAsync(Guid userId, SentimentType sentiment);
        Task<IEnumerable<JournalEntry>> GetJournalEntriesByMoodRatingAsync(Guid userId, int moodRating);
        Task<JournalEntry> GetJournalEntryWithUserAsync(Guid entryId);
    }
} 