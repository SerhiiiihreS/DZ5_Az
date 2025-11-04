
using Azure.Data.Tables;

namespace DZ5_Az.Services.Abstract
{
    public interface IAzureTableService<T> where T : ITableEntity
    {
        public Task<IEnumerable<T>> GetEntitiesAsync(string? category);
        public Task<T> GetEntityAsync(string category, string rowKey);
        public Task<T> UpsertEntityAsync(T entity);
        public Task<bool> DeleteEntityAsync(T entity);
    }
}
