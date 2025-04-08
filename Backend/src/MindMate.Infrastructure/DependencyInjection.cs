using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MindMate.Application.Interfaces;
using MindMate.Core.Interfaces;
using MindMate.Infrastructure.Data;
using MindMate.Infrastructure.JwtGenerator;
using MindMate.Infrastructure.Repositories;

namespace MindMate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Using connection string: {connectionString}");
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
            
            // Register JWT token generator
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
} 