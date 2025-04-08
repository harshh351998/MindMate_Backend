using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindMate.Application.DTOs;
using MindMate.Core.Enums;

namespace MindMate.Application.Interfaces
{
    public interface IJournalEntryService
    {
        Task<IEnumerable<JournalEntryDto>> GetAllJournalEntriesAsync();
        Task<JournalEntryDto> GetJournalEntryByIdAsync(Guid id);
        Task<IEnumerable<JournalEntryDto>> GetJournalEntriesByUserIdAsync(Guid userId);
        Task<IEnumerable<JournalEntryDto>> GetJournalEntriesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<JournalEntryDto>> GetJournalEntriesBySentimentAsync(Guid userId, SentimentType sentiment);
        Task<IEnumerable<JournalEntryDto>> GetJournalEntriesByMoodRatingAsync(Guid userId, int moodRating);
        Task<JournalEntryDto> CreateJournalEntryAsync(JournalEntryCreateDto createDto);
        Task<JournalEntryDto> UpdateJournalEntryAsync(JournalEntryUpdateDto updateDto);
        Task DeleteJournalEntryAsync(Guid id);
        Task<SentimentType> AnalyzeSentimentAsync(string text);
    }
} 