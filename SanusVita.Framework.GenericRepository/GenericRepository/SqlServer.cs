namespace SanusVita.Framework.GenericRepository.GenericRepository;

public class SqlServer<T>(IRepository<SqlServerRepository> repository)
{
    public async Task<int> Create(T entity) => await repository.Insert(entity);
    public async Task Create(List<T> entities) => await repository.Insert(entities);
    public async Task<int> Update(T enitity, int id) => await repository.Update(enitity, id);
    public async Task<List<T>> FindAll() => await repository.FindAll<T>();
    public async Task<List<T>> FindAll(string db = null) => await repository.FindAll<T>(db);
    public async Task<List<T>> FindAll(string db = null, params (string, object)[] parameters) => await repository.FindAll<T>(db, parameters);
    public async Task<List<T>> FindAll(params (string, object)[] parameters) => await repository.FindAll<T>(parameters);
    public async Task<T?> Find() => await repository.Find<T>();
    public async Task<T?> Find(string db = null) => await repository.Find<T>(db);
    public async Task<T?> Find(params (string, object)[] parameters) => await repository.Find<T>(parameters);
    public async Task<int> Delete(int id) => await repository.Delete<T>(id);
    public async Task<int> Delete(string? db = null, params (string, object)[] parameters) => await repository.Delete<T>(db, parameters);
    public async Task<int> Delete(params (string, object)[] parameters) => await repository.Delete<T>(parameters);

}