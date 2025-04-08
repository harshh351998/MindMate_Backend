using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindMate.Application.DTOs;
using MindMate.Application.Interfaces;
using MindMate.Core.Entities;
using MindMate.Core.Enums;
using MindMate.Core.Interfaces;

namespace MindMate.Application.Services
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly IJournalEntryRepository _journalEntryRepository;
        private readonly IUserRepository _userRepository;

        public JournalEntryService(IJournalEntryRepository journalEntryRepository, IUserRepository userRepository)
        {
            _journalEntryRepository = journalEntryRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<JournalEntryDto>> GetAllJournalEntriesAsync()
        {
            var entries = await _journalEntryRepository.GetAllAsync();
            var userIds = entries.Select(e => e.UserId).Distinct().ToList();
            var users = await Task.WhenAll(userIds.Select(id => _userRepository.GetByIdAsync(id)));
            var userDict = users.Where(u => u != null).ToDictionary(u => u.Id, u => u.Username);
            
            return entries.Select(e => {
                string username = "Unknown";
                if (userDict.ContainsKey(e.UserId)) {
                    username = userDict[e.UserId];
                }
                return MapToDto(e, username);
            });
        }

        public async Task<JournalEntryDto> GetJournalEntryByIdAsync(Guid id)
        {
            var entry = await _journalEntryRepository.GetJournalEntryWithUserAsync(id);
            if (entry == null)
                throw new KeyNotFoundException($"Journal entry with ID {id} not found");
                
            return MapToDto(entry, entry.User?.Username ?? "Unknown");
        }

        public async Task<IEnumerable<JournalEntryDto>> GetJournalEntriesByUserIdAsync(Guid userId)
        {
            // First check if the user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var entries = await _journalEntryRepository.GetJournalEntriesByUserIdAsync(userId);
            return entries.Select(e => MapToDto(e, user.Username));
        }

        public async Task<IEnumerable<JournalEntryDto>> GetJournalEntriesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");
                
            var entries = await _journalEntryRepository.GetJournalEntriesByDateRangeAsync(userId, startDate, endDate);
            return entries.Select(e => MapToDto(e, user.Username));
        }

        public async Task<IEnumerable<JournalEntryDto>> GetJournalEntriesBySentimentAsync(Guid userId, SentimentType sentiment)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");
                
            var entries = await _journalEntryRepository.GetJournalEntriesBySentimentAsync(userId, sentiment);
            return entries.Select(e => MapToDto(e, user.Username));
        }

        public async Task<IEnumerable<JournalEntryDto>> GetJournalEntriesByMoodRatingAsync(Guid userId, int moodRating)
        {
            if (moodRating < 1 || moodRating > 5)
                throw new ArgumentOutOfRangeException(nameof(moodRating), "Mood rating must be between 1 and 5");
                
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");
                
            var entries = await _journalEntryRepository.GetJournalEntriesByMoodRatingAsync(userId, moodRating);
            return entries.Select(e => MapToDto(e, user.Username));
        }

        public async Task<JournalEntryDto> CreateJournalEntryAsync(JournalEntryCreateDto createDto)
        {
            var user = await _userRepository.GetByIdAsync(createDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createDto.UserId} not found");
                
            if (createDto.MoodRating < 1 || createDto.MoodRating > 5)
                throw new ArgumentOutOfRangeException(nameof(createDto.MoodRating), "Mood rating must be between 1 and 5");
                
            // Derive sentiment from mood rating (override provided sentiment if any)
            var sentiment = DetermineSentimentFromMood(createDto.MoodRating);
            
            var journalEntry = new JournalEntry
            {
                Id = Guid.NewGuid(),
                UserId = createDto.UserId,
                EntryText = createDto.EntryText,
                MoodRating = createDto.MoodRating,
                Sentiment = sentiment,
                Tags = createDto.Tags ?? new List<string>(),
                IsPrivate = createDto.IsPrivate,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };
            
            await _journalEntryRepository.AddAsync(journalEntry);
            await _journalEntryRepository.SaveChangesAsync();
            
            return MapToDto(journalEntry, user.Username);
        }

        public async Task<JournalEntryDto> UpdateJournalEntryAsync(JournalEntryUpdateDto updateDto)
        {
            var entry = await _journalEntryRepository.GetByIdAsync(updateDto.Id);
            if (entry == null)
                throw new KeyNotFoundException($"Journal entry with ID {updateDto.Id} not found");
                
            if (updateDto.MoodRating < 1 || updateDto.MoodRating > 5)
                throw new ArgumentOutOfRangeException(nameof(updateDto.MoodRating), "Mood rating must be between 1 and 5");
                
            // Update properties
            entry.EntryText = updateDto.EntryText;
            entry.MoodRating = updateDto.MoodRating;
            entry.Tags = updateDto.Tags ?? new List<string>();
            entry.IsPrivate = updateDto.IsPrivate;
            entry.DateModified = DateTime.UtcNow;
            
            // Always derive sentiment from mood rating
            entry.Sentiment = DetermineSentimentFromMood(updateDto.MoodRating);
            
            await _journalEntryRepository.UpdateAsync(entry);
            await _journalEntryRepository.SaveChangesAsync();
            
            var user = await _userRepository.GetByIdAsync(entry.UserId);
            return MapToDto(entry, user?.Username ?? "Unknown");
        }

        public async Task DeleteJournalEntryAsync(Guid id)
        {
            var entry = await _journalEntryRepository.GetByIdAsync(id);
            if (entry == null)
                throw new KeyNotFoundException($"Journal entry with ID {id} not found");
                
            await _journalEntryRepository.DeleteAsync(entry);
            await _journalEntryRepository.SaveChangesAsync();
        }

        public async Task<SentimentType> AnalyzeSentimentAsync(string text)
        {
            // For backward compatibility, we'll still do basic sentiment analysis
            // but with a note that this is deprecated
            Console.WriteLine("WARNING: AnalyzeSentimentAsync is deprecated. Sentiment should be derived from mood rating.");
            
            text = text.ToLower();
            
            string[] positiveWords = { "happy", "good", "great", "excellent", "wonderful", "joy", "excited", "love", "positive", "awesome", "amazing", "fantastic" };
            string[] negativeWords = { "sad", "bad", "terrible", "awful", "miserable", "hate", "angry", "negative", "depressed", "horrible", "annoyed", "frustrated" };
            
            int positiveCount = positiveWords.Count(word => text.Contains(word));
            int negativeCount = negativeWords.Count(word => text.Contains(word));
            
            if (positiveCount > negativeCount)
                return SentimentType.Positive;
            else if (negativeCount > positiveCount)
                return SentimentType.Negative;
            else
                return SentimentType.Neutral;
        }

        private JournalEntryDto MapToDto(JournalEntry entry, string username)
        {
            return new JournalEntryDto
            {
                Id = entry.Id,
                UserId = entry.UserId,
                EntryText = entry.EntryText,
                MoodRating = entry.MoodRating,
                Sentiment = entry.Sentiment,
                Tags = entry.Tags ?? new List<string>(),
                IsPrivate = entry.IsPrivate,
                DateCreated = entry.DateCreated,
                DateModified = entry.DateModified != default ? entry.DateModified : entry.DateCreated, // Fall back to DateCreated if DateModified is not set
                Username = username
            };
        }

        private SentimentType DetermineSentimentFromMood(int moodRating)
        {
            if (moodRating <= 2)
                return SentimentType.Negative;
            else if (moodRating >= 4)
                return SentimentType.Positive;
            else
                return SentimentType.Neutral;
        }
    }
} 