namespace SanusVita.Framework.GenericRepository.GenericRepository;

public interface IRepository<Y> where Y : class
{
    Task<T?> Find<T>(string? db = null);
    Task<T?> Find<T>(params (string, object)[] parameters);
    Task<T?> Find<T>(string? db = null, params (string, object)[] parameters);
    Task<T?> Find<T>(params (string, object, RepositoryEnum)[] parameters);
    
    Task<List<T>> FindAll<T>(string? db = null);
    Task<List<T>> FindAll<T>(params (string, object)[] parameters);
    Task<List<T>> FindAll<T>(params (string, object, RepositoryEnum)[] parameters);
    Task<List<T>> FindAll<T>(string? db = null,params (string, object)[] parameters);
    
    Task<List<T>> FindAllBetween<T>(string column, (string, object) init, (string, object) end);
    Task<T?> FindBetween<T>(string column, (string, object) init, (string, object) end);
    Task<List<T>> FindAllIn<T>(string db = null, params (string, object[])[] inside);
    
    Task<int> Insert<T>(T entity, string db = null);
    Task Insert<T>(List<T> entities, string db = null);
    Task<int> Update<T>(T entity, int id, string db = null);
    Task<TY?> Max<T, TY>(string column, string? db = null, params (string, object)[]? parameters);
    Task<TY?> Min<T, TY>(string column, string? db = null, params (string, object)[]? parameters);
    
    Task<T?> StoreProcedure<T>(string sp, params (string, object)[] parameters);
    Task<List<T>> StoreProcedureList<T>(string sp, params (string, object)[] parameters);
    Task<int> Delete<T>(int id, string? db = null);
    Task<int> Delete<T>(string? db = null, params (string, object)[] parameters);
    Task<int> Delete<T>(params (string, object)[] parameters);
}