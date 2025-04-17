# 🧩 SanusVita.Framework.GenericRepository

A lightweight micro-framework for generic and decoupled data access, compatible with **SQL Server** and **MySQL**, built using **Dapper** and **ADO.NET**. Designed for high-performance .NET applications that require fast and customizable CRUD operations without the overhead of a full ORM like Entity Framework.

---

## 🚀 Key Features

- ✅ Compatible with **SQL Server** and **MySQL**
- 📦 Built on top of **Dapper** (micro ORM)
- 🔄 Full support for generic CRUD operations
- 🔍 Advanced queries: `Find`, `FindAll`, `FindIn`, `FindBetween`
- 🧠 Dynamic expression filtering
- 📊 Support for stored procedures
- 📂 Bulk insert support
- 🧱 Based on the **Generic Repository** pattern
- 📌 Designed for NuGet packaging and reusability

---

## 🛠️ Technologies Used

| Technology     | Description                             |
|----------------|-----------------------------------------|
| C# / .NET 8    | Core programming language and framework |
| Dapper         | Lightweight micro ORM                   |
| ADO.NET        | Low-level database access               |
| SQL Server     | Supported database engine               |
| MySQL          | Supported database engine               |

---

## 🧪 Basic Usage Example

Programs.cs

SQLServer
var connectionString = Environment.GetEnvironmentVariable("SqlServer");
builder.Services.AddTransient<IRepository<SqlServerRepository>>(x => new SqlServerRepository(connectionString!));

MySQL or MariaDB
var connectionString = Environment.GetEnvironmentVariable("MySQL");
builder.Services.AddTransient<IRepository<MySqlRepository>>(x => new MySqlRepository(connectionString!));

class query

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
                response.StatusCode = (int)HttpStatusCode.NoContent;
                response.Message = language == Language.English
                    ? "There are no genders available to display."
                    : "No hay generos disponibles para mostrar.";
                response.Entity = null;
                return response;
            }
            
            response.StatusCode = 200;
            response.Message = language == Language.English
                ? "Genders retrieved successfully."
                : "Se obtuvieron los generos correctamente.";
            response.Entity = get;
            return response;
        }
        catch (Exception e)
        {
            var language = languageService.GetLanguageFromHeader();
            response.StatusCode = 500;
            response.Error = e.Message;
            response.Message = language == Language.English
                ? "An error occurred while retrieving the data."
                : "Error al obtener los datos.";
            response.Entity = null;
            return response;
        }
    }
}

