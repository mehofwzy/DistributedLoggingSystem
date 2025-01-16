using DLS.Server.Models;

namespace DLS.Server.Services
{
    public interface IStorageBackend
    {
        Task StoreLogAsync(LogEntry log);
        Task<(int TotalCount, IEnumerable<LogEntry> Logs)> RetrieveLogsAsync(string? service, string? level,
            DateTime? startTime, DateTime? endTime, int page = 1, int page_size = 5);
    }
}