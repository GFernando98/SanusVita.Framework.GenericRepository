using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SanusVita.Framework.GenericRepository.GenericRepository;

public class SqlServerRepository : BaseRepository, IRepository<SqlServerRepository>
{
    private readonly string _connectionString;

    public SqlServerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

   public async Task<T?> Find<T>(string? db = null)
    {
        var select = GenerateSelect<T>(db);
        
        return await SelectEntity<T>($"{select};");
    }

    public async Task<T?> Find<T>(params (string, object)[] parameters)
    {
        var select = GenerateSelect<T>();
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);
        
        return await SelectEntity<T>($"{select} {where}", dynamicParameters);
    }

    public async Task<T?> Find<T>(params (string, object, RepositoryEnum)[] parameters)
    {
        var select = GenerateSelect<T>();
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);
        
        return await SelectEntity<T>($"{select} {where}", dynamicParameters);
    }

    public async Task<T?> Find<T>(string? db = null, params (string, object)[] parameters)
    {
        var select = GenerateSelect<T>(db);
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);
        
        return await SelectEntity<T>($"{select} {where}", dynamicParameters);
    }

    public async Task<T?> FindBetween<T>(string column, (string, object) init, (string, object) end)
    {
        var select = GenerateSelect<T>();
        
        var where = GenerateWhereBetweenParams(column, init, end);
        
        var dynamicParameters = ToParameters(init, end);
        
        return await SelectEntity<T>($"{select} {where}", dynamicParameters);
    }

    public async Task<List<T>> FindAll<T>(string? db = null)
    {
        var select = GenerateSelect<T>(db);
        
        return await SelectEntities<T>($"{select};");
    }

    public async Task<List<T>> FindAll<T>(params (string, object)[] parameters)
    {
        var select = GenerateSelect<T>();
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);
        
        return await SelectEntities<T>($"{select} {where};", dynamicParameters);
    }

    public async Task<List<T>> FindAll<T>(string? db = null, params (string, object)[] parameters)
    {
        var select = GenerateSelect<T>(db);
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);
        
        return await SelectEntities<T>($"{select} {where};", dynamicParameters);
    }

    public async Task<List<T>> FindAll<T>(params (string, object, RepositoryEnum)[] parameters)
    {
        var select = GenerateSelect<T>();
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);
        
        return await SelectEntities<T>($"{select} {where};", dynamicParameters);
    }

    public async Task<List<T>> FindAllBetween<T>(string column, (string, object) init, (string, object) end)
    {
        var select = GenerateSelect<T>();
        
        var where = GenerateWhereBetweenParams(column, init, end);
        
        var dynamicParameters = ToParameters(init, end);
        
        return await SelectEntities<T>($"{select} {where}", dynamicParameters);
    }

    public async Task<List<T>> FindAllIn<T>((string, object[]) inside, string db = null)
    {
        var select = GenerateSelect<T>(db);
        
        var where = GenerateWhereInParams(inside);
        
        var dynamicParameters = ToParameters(inside);
        
        using (var command = new SqlConnection(_connectionString))
        {
            var list = await command.QueryAsync<T>($"{select} {where}", dynamicParameters);
            return list.ToList();
        }
    }

    public async Task<List<T>> FindAllIn<T>(string db = null, params (string, object[])[] inside)
    {
        var select = GenerateSelect<T>(db);
        
        var where = GenerateWhereInParams(inside);
        
        var dynamicParameters = ToParameters(inside);
        
        using (var command = new SqlConnection(_connectionString))
        {
            var list = await command.QueryAsync<T>($"{select} {where}", dynamicParameters);
            return list.ToList();
        }
    }

    // Insert
    
    public async Task<int> Insert<T>(T entity, string db = null)
    {
        var insert = GenerateInsert<T>(DatabaseEnum.MySql, db);
        
        var dictionary = ToDictionary(entity);
        
        var dynamicParameters = ToParameters(dictionary);
        
        return await ExecuteInsertUpdate(insert, dynamicParameters, true);
    }

    public async Task Insert<T>(List<T> entities, string db = null!)
    {
        var insert = GenerateInsertMassive(entities, db);
        
        var dynamicParameters = ToParameters(insert.parameters);
        
        await ExecuteInsertMassive(insert.query, dynamicParameters);
    }

    // Update

    public async Task<int> Update<T>(T entity, int id, string db = null!)
    {
        var update = GenerateUpdate<T>(db);
        
        var parameters = ToDictionary(entity);
        
        parameters["Id"] = id;
        
        var where = GenerateWhereUpdate();
        
        var dynamicParameters = ToParameters(parameters);
        
        await ExecuteUpdate($"{update} {where}", dynamicParameters, false);
        
        return id;
    }

    public async Task<T?> StoreProcedure<T>(string sp, params (string, object)[] parameters)
    {
        var dynamicParameters = ToParameters(parameters);
        
        using (var command = new SqlConnection(_connectionString))
            return await command.QuerySingleOrDefaultAsync<T>(sp, dynamicParameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<List<T>> StoreProcedureList<T>(string sp, params (string, object)[] parameters)
    {
        var dynamicParameters = ToParameters(parameters);
        
        using (var command = new SqlConnection(_connectionString))
        {
            var list = await command.QueryAsync<T>(sp, dynamicParameters, commandType: CommandType.StoredProcedure);
            return list.ToList();
        }
    }

    // Max and Min

    public async Task<TY?> Max<T, TY>(string column, string? db = null, params (string, object)[]? parameters)
    {
        var table = typeof(T).Name;
        
        var query = db is null ? $"SELECT MAX({column}) FROM {table}" : $"SELECT MAX({column}) FROM {db}.{table}";
        
        using (var command = new SqlConnection(_connectionString))
        {
            if (parameters.Any())
            {
                var generateWhere = GenerateWhereParams(parameters);
                
                query = $"{query} {generateWhere}";
                
                var dynamicParameters = ToParameters(parameters);
                
                return await command.QuerySingleOrDefaultAsync<TY>(query, dynamicParameters);
            }
            
            return await command.QuerySingleOrDefaultAsync<TY>(query);
        }
    }

    public async Task<TY?> Min<T, TY>(string column, string? db = null, params (string, object)[]? parameters)
    {
        var table = typeof(T).Name;
        
        var query = db is null ? $"SELECT MIN({column}) FROM {table}" : $"SELECT MIN({column}) FROM {db}.{table}";
        
        using (var command = new SqlConnection(_connectionString))
        {
            if (parameters.Any())
            {
                var generateWhere = GenerateWhereParams(parameters);
                
                query = $"{query} {generateWhere}";
                
                var dynamicParameters = ToParameters(parameters);
                
                return await command.QuerySingleOrDefaultAsync<TY>(query, dynamicParameters);
            }
            
            return await command.QuerySingleOrDefaultAsync<TY>(query);
        }
    }
    
    //Delete
    
    public async Task<int> Delete<T>(params (string, object)[] parameters)
    {
        return await Delete<T>(null, parameters);
    }
    
    public async Task<int> Delete<T>(int id, string? db = null)
    {
        var delete = GenerateDelete<T>(db);
        
        var where = GenerateWhereDelete();
        
        var parameters = new DynamicParameters();
        
        parameters.Add("Id", id);

        using var connection = new SqlConnection(_connectionString);
        
        return await connection.ExecuteAsync($"{delete} {where};", parameters);
    }
    
    public async Task<int> Delete<T>(string? db = null, params (string, object)[] parameters)
    {
        var delete = GenerateDelete<T>(db);
        
        var where = GenerateWhereParams(parameters);
        
        var dynamicParameters = ToParameters(parameters);

        using var connection = new SqlConnection(_connectionString);
        
        return await connection.ExecuteAsync($"{delete} {where};", dynamicParameters);
    }

    // Select and Execute

    private async Task<T?> SelectEntity<T>(string query)
    {
        using (var command = new SqlConnection(_connectionString))
            return await command.QuerySingleOrDefaultAsync<T>(query);
    }

    private async Task<List<T>> SelectEntities<T>(string query)
    {
        using (var command = new SqlConnection(_connectionString))
        {
            var select = await command.QueryAsync<T>(query);
            
            return select.ToList();
        }
    }

    private async Task<T?> SelectEntity<T>(string query, DynamicParameters parameters)
    {
        using (var command = new SqlConnection(_connectionString))
            return await command.QuerySingleOrDefaultAsync<T>(query, parameters);
    }

    private async Task<List<T>> SelectEntities<T>(string query, DynamicParameters parameters)
    {
        using (var command = new SqlConnection(_connectionString))
        {
            var select = await command.QueryAsync<T>(query, parameters);
            
            return select.ToList();
        }
    }

    private async Task ExecuteUpdate(string query, DynamicParameters parameters, bool insert)
    {
        using (var command = new SqlConnection(_connectionString))
            await command.ExecuteAsync(query, parameters);
    }

    private async Task<int> ExecuteInsertUpdate(string query, DynamicParameters parameters, bool insert)
    {
        using (var command = new SqlConnection(_connectionString))
            return await command.QuerySingleOrDefaultAsync<int>(query, parameters); 
    }

    private async Task ExecuteInsertMassive(string query, DynamicParameters parameters)
    {
        using (var command = new SqlConnection(_connectionString))
            await command.QuerySingleOrDefaultAsync(query, parameters); 
    }
}