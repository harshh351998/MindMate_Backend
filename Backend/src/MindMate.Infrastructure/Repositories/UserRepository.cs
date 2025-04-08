using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindMate.Core.Entities;
using MindMate.Core.Interfaces;
using MindMate.Infrastructure.Data;

namespace MindMate.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        // Get a user by username
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        // Get a user with their journal entries
        public async Task<User> GetUserWithJournalEntriesAsync(Guid userId)
        {
            return await _dbSet
                .Include(u => u.JournalEntries)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Check if a username is already taken
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }
    }
} 