using Azure;
using Azure.Data.Tables;


namespace DZ5_Az.Models
{

    public class Book : ITableEntity
    {
        public string? Title { get; set; } = default!;
        public string? Author { get; set; } = default!;
        public int Pages { get; set; }
        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
