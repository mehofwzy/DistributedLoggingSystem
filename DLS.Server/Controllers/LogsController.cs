using DLS.Server.Models;
using DLS.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DLS.Server.Controllers
{
    [ApiController]
    [Route("v1/logs")]
    public class LogsController : ControllerBase
    {
        private readonly IStorageBackend _storageBackend;

        public LogsController(IStorageBackend storageBackend)
        {
            _storageBackend = storageBackend;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> StoreLog([FromBody] LogEntry log)
        {
            if (log == null || string.IsNullOrEmpty(log.Service) || string.IsNullOrEmpty(log.Level) || string.IsNullOrEmpty(log.Message))
            {
                return BadRequest("Invalid log entry");
            }

            log.Timestamp = log.Timestamp.ToUniversalTime(); // Normalize timestamp
            await _storageBackend.StoreLogAsync(log);
            return Ok("Log stored successfully");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLogs([FromQuery] string? service, [FromQuery] string? level, 
            [FromQuery] DateTime? startTime, [FromQuery] DateTime? endTime, int page = 1, int page_size = 5)
        {
            var result = await _storageBackend.RetrieveLogsAsync(service, level, startTime, endTime, page, page_size);

            var response = new
            {
                result.Logs,
                result.TotalCount
            };

            return Ok(response);
        }
    }
}
