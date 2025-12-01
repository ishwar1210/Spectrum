using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly string _connectionString;

    public LocationRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Location?> GetByIdAsync(int locationId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT LocationId, Loction_name AS LocationName, L_Descripation AS Description, CreatedDate, UpdatedDate, IsActive, Created_by AS CreatedBy FROM tblLocation WHERE LocationId = @LocationId";
        return await connection.QueryFirstOrDefaultAsync<Location>(sql, new { LocationId = locationId });
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT LocationId, Loction_name AS LocationName, L_Descripation AS Description, CreatedDate, UpdatedDate, IsActive, Created_by AS CreatedBy FROM tblLocation ORDER BY Loction_name";
        return await connection.QueryAsync<Location>(sql);
    }

    public async Task<IEnumerable<Location>> GetActiveAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT LocationId, Loction_name AS LocationName, L_Descripation AS Description, CreatedDate, UpdatedDate, IsActive, Created_by AS CreatedBy FROM tblLocation WHERE IsActive = 1 ORDER BY Loction_name";
        return await connection.QueryAsync<Location>(sql);
    }

    public async Task<int> CreateAsync(Location location)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblLocation (Loction_name, L_Descripation, CreatedDate, IsActive, Created_by)
                    VALUES (@LocationName, @Description, GETDATE(), @IsActive, @CreatedBy);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, location);
    }

    public async Task<bool> UpdateAsync(int locationId, Location location)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblLocation SET Loction_name = @LocationName, L_Descripation = @Description, IsActive = @IsActive, UpdatedDate = GETDATE() WHERE LocationId = @LocationId";
        var parameters = new
        {
            LocationId = locationId,
            location.LocationName,
            location.Description,
            location.IsActive
        };
        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int locationId)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblLocation WHERE LocationId = @LocationId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { LocationId = locationId });
        return rowsAffected > 0;
    }

    public async Task<bool> LocationNameExistsAsync(string locationName, int? excludeLocationId = null)
    {
        using var connection = CreateConnection();
        var sql = excludeLocationId.HasValue
            ? "SELECT COUNT(1) FROM tblLocation WHERE Loction_name = @LocationName AND LocationId != @ExcludeLocationId"
            : "SELECT COUNT(1) FROM tblLocation WHERE Loction_name = @LocationName";

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            LocationName = locationName,
            ExcludeLocationId = excludeLocationId
        });
        return count > 0;
    }
}
