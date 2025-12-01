using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly string _connectionString;

    public RoleRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Role?> GetByIdAsync(int roleId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT RoleId, Role_Name AS RoleName, CreatedDate, UpdatedDate FROM tblRoles WHERE RoleId = @RoleId";
        return await connection.QueryFirstOrDefaultAsync<Role>(sql, new { RoleId = roleId });
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT RoleId, Role_Name AS RoleName, CreatedDate, UpdatedDate FROM tblRoles ORDER BY Role_Name";
        return await connection.QueryAsync<Role>(sql);
    }

    public async Task<int> CreateAsync(Role role)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblRoles 
                    (Role_Name, CreatedDate)
                    VALUES 
                    (@RoleName, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        
        return await connection.ExecuteScalarAsync<int>(sql, role);
    }

    public async Task<bool> UpdateAsync(int roleId, Role role)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblRoles 
                    SET Role_Name = @RoleName,
                        UpdatedDate = GETDATE()
                    WHERE RoleId = @RoleId";
        
        var parameters = new
        {
            RoleId = roleId,
            role.RoleName
        };

        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int roleId)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblRoles WHERE RoleId = @RoleId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { RoleId = roleId });
        return rowsAffected > 0;
    }

    public async Task<bool> RoleNameExistsAsync(string roleName, int? excludeRoleId = null)
    {
        using var connection = CreateConnection();
        var sql = excludeRoleId.HasValue
            ? "SELECT COUNT(1) FROM tblRoles WHERE Role_Name = @RoleName AND RoleId != @ExcludeRoleId"
            : "SELECT COUNT(1) FROM tblRoles WHERE Role_Name = @RoleName";

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            RoleName = roleName,
            ExcludeRoleId = excludeRoleId
        });
        return count > 0;
    }
}