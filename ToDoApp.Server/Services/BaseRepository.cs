using NPoco;
using NPoco.Linq;
using ToDoApp.Server.Contracts;

namespace ToDoApp.Server.Services;

public class BaseRepository<T>(IDatabase database) : IBaseRepository<T> where T : class
{
    public async Task<T> GetByIdAsync(int id)
    {
        return await database.SingleOrDefaultByIdAsync<T>(id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await database.FetchAsync<T>().ConfigureAwait(false);
    }

    public async Task AddAsync(T entity)
    {
        using var transaction = database.GetTransaction();
        await database.InsertAsync(entity);
        transaction.Complete();
    }

    public async Task UpdateAsync(T entity)
    {
        using var transaction = database.GetTransaction();
        await database.UpdateAsync(entity);
        transaction.Complete();
    }

    public async Task DeleteAsync(int id)
    {
        using var transaction = database.GetTransaction();
        var entity = await database.SingleOrDefaultByIdAsync<T>(id);
        if (entity != null)
        {
            await database.DeleteAsync(entity);
        }
        transaction.Complete();
    }

    public async Task<IEnumerable<T>> QueryToGetRecordAsync(string sql, params object[] args)
    {
        return await database.FetchAsync<T>(sql, args);
    }

    public IAsyncQueryProviderWithIncludes<T> QueryAsync()
    {
        return database.QueryAsync<T>();
    }

    public async Task ExecuteSqlAsync(string sql, params object[] args)
    {
        await database.ExecuteAsync(sql, args).ConfigureAwait(false);
    }

}
