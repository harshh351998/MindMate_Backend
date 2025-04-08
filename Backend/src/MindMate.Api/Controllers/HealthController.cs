using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindMate.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace MindMate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public HealthController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                // Check if we can connect to the database
                bool canConnectToDb = await _dbContext.Database.CanConnectAsync();

                var response = new
                {
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Database = canConnectToDb ? "Connected" : "Disconnected",
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
                };

                if (!canConnectToDb)
                {
                    return StatusCode(503, response); // Service Unavailable if DB is down
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Unhealthy",
                    Timestamp = DateTime.UtcNow,
                    Error = ex.Message
                });
            }
        }
    }
}
