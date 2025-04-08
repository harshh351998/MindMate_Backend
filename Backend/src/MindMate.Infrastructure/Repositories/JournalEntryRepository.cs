using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindMate.Core.Entities;
using MindMate.Core.Enums;
using MindMate.Core.Interfaces;
using MindMate.Infrastructure.Data;

namespace MindMate.Infrastructure.Repositories
{
    public class JournalEntryRepository : Repository<JournalEntry>, IJournalEntryRepository
    {
        public JournalEntryRepository(AppDbContext context) : base(context)
        {
        }

        // Get all journal entries for a specific user
        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.DateCreated)
                .ToListAsync();
        }

        // Get journal entries within a date range for a specific user
        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(j => j.UserId == userId && j.DateCreated >= startDate && j.DateCreated <= endDate)
                .OrderByDescending(j => j.DateCreated)
                .ToListAsync();
        }

        // Get journal entries by sentiment type for a specific user
        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesBySentimentAsync(Guid userId, SentimentType sentiment)
        {
            return await _dbSet
                .Where(j => j.UserId == userId && j.Sentiment == sentiment)
                .OrderByDescending(j => j.DateCreated)
                .ToListAsync();
        }

        // Get journal entries by mood rating for a specific user
        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesByMoodRatingAsync(Guid userId, int moodRating)
        {
            return await _dbSet
                .Where(j => j.UserId == userId && j.MoodRating == moodRating)
                .OrderByDescending(j => j.DateCreated)
                .ToListAsync();
        }

        // Get a journal entry with its associated user
        public async Task<JournalEntry> GetJournalEntryWithUserAsync(Guid entryId)
        {
            return await _dbSet
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == entryId);
        }
    }
} 