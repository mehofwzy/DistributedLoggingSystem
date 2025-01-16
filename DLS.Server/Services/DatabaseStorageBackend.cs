using DLS.Server.Data;
using DLS.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DLS.Server.Services
{
    public class DatabaseStorageBackend : IStorageBackend
    {
        private readonly AppDbContext _context;

        public DatabaseStorageBackend(AppDbContext context)
        {
            _context = context;
        }

        public async Task StoreLogAsync(LogEntry log)
        {
            await _context.LogEntries.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<(int TotalCount, IEnumerable<LogEntry> Logs)> RetrieveLogsAsync(string? service, string? level,
            DateTime? startTime, DateTime? endTime, int page = 1, int page_size = 5)
        {
            var query = _context.LogEntries.AsQueryable();

            if (!string.IsNullOrEmpty(service))
                query = query.Where(log => log.Service == service);

            if (!string.IsNullOrEmpty(level))
                query = query.Where(log => log.Level == level);

            if (startTime.HasValue)
                query = query.Where(log => log.Timestamp >= startTime);

            if (endTime.HasValue)
                query = query.Where(log => log.Timestamp <= endTime);


            int totalCount = await query.CountAsync();

            //for pagination
            query = query
                  .OrderByDescending(l => l.Timestamp)
                  .Skip((page - 1) * page_size)
                  .Take(page_size);

            return  (totalCount, await query.ToListAsync());
        }
    }
}
