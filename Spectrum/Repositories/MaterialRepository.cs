using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly string _connectionString;

    public MaterialRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Material?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblMaterial WHERE MaterialId = @Id";
        return await connection.QueryFirstOrDefaultAsync<Material>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Material>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblMaterial ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<Material>(sql);
    }

    public async Task<int> CreateAsync(Material m)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblMaterial (Material_Name, Material_Code, Material_Status, Material_VisitorId, Material_EntryDate, CreatedDate)
                    VALUES (@Material_Name, @Material_Code, @Material_Status, @Material_VisitorId, @Material_EntryDate, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, m);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, Material m)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblMaterial SET Material_Name = @Material_Name, Material_Code = @Material_Code, Material_Status = @Material_Status, Material_VisitorId = @Material_VisitorId, Material_EntryDate = @Material_EntryDate, UpdatedDate = GETDATE() WHERE MaterialId = @Id";
        var parameters = new
        {
            Id = id,
            m.Material_Name,
            m.Material_Code,
            m.Material_Status,
            m.Material_VisitorId,
            m.Material_EntryDate
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblMaterial WHERE MaterialId = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
