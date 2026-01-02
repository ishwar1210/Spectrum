using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly string _connectionString;

    public DepartmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Department?> GetByIdAsync(int departmentId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT DepartmentId, DepartmentName, CreatedDate, UpdatedDate, IsActive FROM tblDepartment WHERE DepartmentId = @DepartmentId";
        return await connection.QueryFirstOrDefaultAsync<Department>(sql, new { DepartmentId = departmentId });
    }

    public async Task<Department?> GetByNameAsync(string departmentName)
    {
        using var connection = CreateConnection();
        var sql = "SELECT DepartmentId, DepartmentName, CreatedDate, UpdatedDate, IsActive FROM tblDepartment WHERE DepartmentName = @DepartmentName";
        return await connection.QueryFirstOrDefaultAsync<Department>(sql, new { DepartmentName = departmentName });
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT DepartmentId, DepartmentName, CreatedDate, UpdatedDate, IsActive FROM tblDepartment ORDER BY DepartmentName";
        return await connection.QueryAsync<Department>(sql);
    }

    public async Task<IEnumerable<Department>> GetActiveAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT DepartmentId, DepartmentName, CreatedDate, UpdatedDate, IsActive FROM tblDepartment WHERE IsActive = 1 ORDER BY DepartmentName";
        return await connection.QueryAsync<Department>(sql);
    }

    public async Task<int> CreateAsync(Department department)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblDepartment 
                    (DepartmentName, CreatedDate, IsActive)
                    VALUES 
                    (@DepartmentName, GETDATE(), 1);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        
        return await connection.ExecuteScalarAsync<int>(sql, department);
    }

    public async Task<bool> UpdateAsync(int departmentId, Department department)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblDepartment 
                    SET DepartmentName = @DepartmentName,
                        IsActive = @IsActive,
                        UpdatedDate = GETDATE()
                    WHERE DepartmentId = @DepartmentId";
        
        var parameters = new
        {
            DepartmentId = departmentId,
            department.DepartmentName,
            department.IsActive
        };

        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<bool> DepartmentNameExistsAsync(string departmentName, int? excludeDepartmentId = null)
    {
        using var connection = CreateConnection();
        var sql = excludeDepartmentId.HasValue
            ? "SELECT COUNT(1) FROM tblDepartment WHERE DepartmentName = @DepartmentName AND DepartmentId != @ExcludeDepartmentId"
            : "SELECT COUNT(1) FROM tblDepartment WHERE DepartmentName = @DepartmentName";

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            DepartmentName = departmentName,
            ExcludeDepartmentId = excludeDepartmentId
        });
        return count > 0;
    }

    public async Task<bool> DeleteAsync(int departmentId)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblDepartment WHERE DepartmentId = @DepartmentId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { DepartmentId = departmentId });
        return rowsAffected > 0;
    }
}