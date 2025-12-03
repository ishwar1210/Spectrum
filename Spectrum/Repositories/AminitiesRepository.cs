using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class AminitiesRepository : IAminitiesRepository
{
    private readonly string _connectionString;

    public AminitiesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Aminities?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblAminities WHERE AminitiesId = @Id";
        return await connection.QueryFirstOrDefaultAsync<Aminities>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Aminities>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblAminities ORDER BY Aminities_Name";
        return await connection.QueryAsync<Aminities>(sql);
    }

    public async Task<int> CreateAsync(Aminities a)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblAminities (Aminities_Name, Aminities_isActive, CreatedDate)
                    VALUES (@Aminities_Name, @Aminities_isActive, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, a);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, Aminities a)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblAminities SET Aminities_Name = @Aminities_Name, Aminities_isActive = @Aminities_isActive, UpdatedDate = GETDATE() WHERE AminitiesId = @Id";
        var parameters = new
        {
            Id = id,
            a.Aminities_Name,
            a.Aminities_isActive
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblAminities WHERE AminitiesId = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
