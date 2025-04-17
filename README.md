# 🧩 SanusVita.Framework.GenericRepository

A lightweight and modular micro-framework for generic, high-performance, and database-agnostic data access. Built on top of **Dapper** and **ADO.NET**, this framework simplifies CRUD operations and advanced querying in **SQL Server** and **MySQL** without the complexity of a full ORM like Entity Framework.

---

## 🚀 Key Features

- ✅ Plug-and-play support for **SQL Server** and **MySQL**/**MariaDB**
- ⚡ Built on **Dapper** for minimal overhead and fast execution
- 🔁 Full support for **Generic CRUD** operations
- 🔍 Advanced queries: `Find`, `FindAll`, `FindIn`, `FindBetween`
- 🧠 Dynamic filtering using expression trees
- 📊 Native support for stored procedures
- 📥 Bulk insert operations
- 🧱 Implements the **Generic Repository Pattern**
- 📦 Ready for **NuGet** distribution and reuse

---

## 🛠️ Technologies Used

| Technology     | Purpose                                   |
|----------------|-------------------------------------------|
| C# / .NET 8    | Core framework and language               |
| Dapper         | Micro ORM for high-performance queries    |
| ADO.NET        | Direct database communication             |
| SQL Server     | Supported database engine                 |
| MySQL          | Supported database engine                 |

---

## 🧪 Usage Example

### 🔧 Register the Repository - Program.cs

```csharp
// For SQL Server
var sqlConnectionString = Environment.GetEnvironmentVariable("SqlServer");
builder.Services.AddTransient<IRepository<SqlServerRepository>>(x =>
    new SqlServerRepository(sqlConnectionString!));

// For MySQL or MariaDB
var mysqlConnectionString = Environment.GetEnvironmentVariable("MySQL");
builder.Services.AddTransient<IRepository<MySqlRepository>>(x =>
    new MySqlRepository(mysqlConnectionString!));

```

## Example use

```csharp
public class Genders
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public bool State { get; set; }
}

public class GendersQuery(IRepository<SqlServerRepository> query, LanguageService languageService)
{
    public async Task<Response<List<Genders>>> GetGenders()
    {
        var response = new Response<List<Genders>>();
        try
        {
            var language = languageService.GetLanguageFromHeader();
            var get = await query.FindAll<Genders>();

            if (get.Count == 0)
            {
                response.StatusCode = 204;
                response.Message = language == Language.English
                    ? "There are no genders available to display."
                    : "No hay géneros disponibles para mostrar.";
                return response;
            }

            response.StatusCode = 200;
            response.Message = language == Language.English
                ? "Genders retrieved successfully."
                : "Se obtuvieron los géneros correctamente.";
            response.Entity = get;
            return response;
        }
        catch (Exception e)
        {
            var language = languageService.GetLanguageFromHeader();
            response.StatusCode = 500;
            response.Message = language == Language.English
                ? "An error occurred while retrieving the data."
                : "Ocurrió un error al obtener los datos.";
            response.Error = e.Message;
            return response;
        }
    }
}
```
