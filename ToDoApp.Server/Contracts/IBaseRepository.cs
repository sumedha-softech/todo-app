using NPoco.Linq;

namespace ToDoApp.Server.Contracts;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Get an entity by its ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// Get all entities asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Add a new entity asynchronously.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Update an existing entity asynchronously.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Delete an entity by its ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// get result by query asynchronously.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> QueryToGetRecordAsync(string sql, params object[] args);

    /// <summary>
    /// get result by query asynchronously.
    /// </summary>
    /// <returns></returns>
    IAsyncQueryProviderWithIncludes<T> QueryAsync();

    /// <summary>  
    /// Execute a raw SQL command asynchronously.  
    /// </summary>  
    /// <param name="sql">The SQL command to execute.</param>  
    /// <param name="args">The parameters for the SQL command.</param>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    Task ExecuteSqlAsync(string sql, params object[] args);
}
