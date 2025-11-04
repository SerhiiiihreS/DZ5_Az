using Azure.Data.Tables;
using DZ5_Az.Models;
using DZ5_Az.Services.Abstract;



namespace DZ5_Az.Services.Implementation
{
    public class MyAzureBookService: IAzureBookService
    {
        private readonly string connectionString;

        public MyAzureBookService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureTable")
                                      ?? throw new InvalidOperationException("AzureWebJobsStorage is not set");
        }

        public async Task<TableClient> GetTableClient(string connectionString)
        {
            TableServiceClient tableService = new TableServiceClient(connectionString);
            TableClient tableClient = tableService.GetTableClient("Books");
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public async Task<IEnumerable<Book>> GetEntitiesAsync(string? category)
        {
            TableClient tableClient = await GetTableClient(connectionString);
            List<Book> books = new List<Book>();
            if (category is not null)
            {
                await foreach (Book book in tableClient.QueryAsync<Book>(t => t.PartitionKey == category))
                {
                    books.Add(book);
                }
            }
            else
            {
                await foreach (Book book in tableClient.QueryAsync<Book>())
                {
                    books.Add(book);
                }
            }
            return books;
        }

        public async Task<Book> GetEntityAsync(string category, string rowKey)
        {
            TableClient tableClient = await GetTableClient(connectionString);
            Book book = await tableClient.GetEntityAsync<Book>(category, rowKey);
            return book;
        }

        public async Task<Book> UpsertEntityAsync(Book entity)
        {
            TableClient tableClient = await GetTableClient(connectionString);
            await tableClient.UpsertEntityAsync<Book>(entity);
            return entity;
        }

        public async Task<bool> DeleteEntityAsync(Book entity)
        {
            TableClient tableClient = await GetTableClient(connectionString);
            var resp = await tableClient.DeleteEntityAsync(entity);
            return !resp.IsError;
        }
    }
}
