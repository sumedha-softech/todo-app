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
    Task<IEnumerable<T>> QueryAsync(string sql, params object[] args);

    /// <summary>
    /// get result by query asynchronously.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    IAsyncQueryProviderWithIncludes<T> QueryNewAsync();

}
