namespace DLS.Server.Models
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public string Service { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string BackendType { get; set; } // Database, S3, FileSystem
    }
}
