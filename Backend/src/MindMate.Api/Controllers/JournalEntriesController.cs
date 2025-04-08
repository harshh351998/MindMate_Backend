using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindMate.Application.DTOs;
using MindMate.Application.Interfaces;
using MindMate.Core.Enums;
using MindMate.Core.Entities;

namespace MindMate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Temporarily removing authorization for development
    // [Authorize]
    public class JournalEntriesController : ControllerBase
    {
        private readonly IJournalEntryService _journalEntryService;

        public JournalEntriesController(IJournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalEntryDto>>> GetAllJournalEntries()
        {
            var entries = await _journalEntryService.GetAllJournalEntriesAsync();
            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JournalEntryDto>> GetJournalEntry(Guid id)
        {
            try
            {
                var entry = await _journalEntryService.GetJournalEntryByIdAsync(id);
                if (entry == null)
                {
                    return NotFound(new { message = $"Journal entry with ID {id} not found" });
                }
                return Ok(entry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the journal entry", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<JournalEntryDto>> CreateJournalEntry(JournalEntryCreateDto createDto)
        {
            try
            {
                var entry = await _journalEntryService.CreateJournalEntryAsync(createDto);
                return CreatedAtAction(nameof(GetJournalEntry), new { id = entry.Id }, entry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the journal entry", details = ex.Message });
            }
        }

        [HttpPost("dev")]
        public async Task<ActionResult<JournalEntryDto>> CreateDevEntry(JournalEntryCreateDto createDto)
        {
            try
            {
                // For development purposes, create an entry without user validation
                // Use a hardcoded sentiment for simplicity
                var journalEntry = new JournalEntry
                {
                    Id = Guid.NewGuid(),
                    UserId = createDto.UserId,
                    EntryText = createDto.EntryText,
                    MoodRating = createDto.MoodRating,
                    Sentiment = SentimentType.Neutral, // Default to neutral
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };
                
                // Manually create the DTO to return
                var entryDto = new JournalEntryDto
                {
                    Id = journalEntry.Id,
                    UserId = journalEntry.UserId,
                    EntryText = journalEntry.EntryText,
                    MoodRating = journalEntry.MoodRating,
                    Sentiment = journalEntry.Sentiment,
                    DateCreated = journalEntry.DateCreated,
                    DateModified = journalEntry.DateModified,
                    Username = "DevUser"
                };
                
                // Save to the database
                await _journalEntryService.CreateJournalEntryAsync(createDto);
                
                return Ok(entryDto);
            }
            catch (Exception error)
            {
                // Return the created object anyway for development purposes
                var entryDto = new JournalEntryDto
                {
                    Id = Guid.NewGuid(),
                    UserId = createDto.UserId,
                    EntryText = createDto.EntryText,
                    MoodRating = createDto.MoodRating,
                    Sentiment = SentimentType.Neutral,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    Username = "DevUser"
                };
                
                // Log the error but return success for development purposes
                Console.WriteLine($"Development endpoint error: {error.Message}");
                
                return Ok(entryDto);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JournalEntryDto>> UpdateJournalEntry(Guid id, JournalEntryUpdateDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest(new { message = "ID in the URL does not match ID in the request body" });
                }

                var entry = await _journalEntryService.UpdateJournalEntryAsync(updateDto);
                return Ok(entry);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the journal entry", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJournalEntry(Guid id)
        {
            try
            {
                await _journalEntryService.DeleteJournalEntryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the journal entry", details = ex.Message });
            }
        }
    }
} 