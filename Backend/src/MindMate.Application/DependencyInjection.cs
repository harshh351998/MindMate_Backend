using Microsoft.Extensions.DependencyInjection;
using MindMate.Application.Interfaces;
using MindMate.Application.Services;

namespace MindMate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJournalEntryService, JournalEntryService>();
            services.AddScoped<IAuthService, AuthService>();
            
            return services;
        }
    }
} 