using System;
using System.Threading.Tasks;
using MindMate.Core.Entities;

namespace MindMate.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetUserWithJournalEntriesAsync(Guid userId);
        Task<bool> UsernameExistsAsync(string username);
    }
} 