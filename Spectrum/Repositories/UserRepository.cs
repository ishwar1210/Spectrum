using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblUsers WHERE Username = @Username";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblUsers WHERE UserId = @UserId";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblUsers WHERE U_Name = @Name";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Name = name });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblUsers ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<int> CreateAsync(User user)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblUsers 
                    (Username, Password_hash, U_Name, U_Mobile, U_Email, U_Address, 
                     U_RoleId, U_DepartmentID, U_ReportingToId, CreatedDate)
                    VALUES 
                    (@Username, @Password_hash, @U_Name, @U_Mobile, @U_Email, @U_Address, 
                     @U_RoleId, @U_DepartmentID, @U_ReportingToId, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        
        var result = await connection.ExecuteScalarAsync<int?>(sql, user);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int userId, User user)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblUsers 
                    SET Username = @Username,
                        U_Name = @U_Name,
                        U_Mobile = @U_Mobile,
                        U_Email = @U_Email,
                        U_Address = @U_Address,
                        U_RoleId = @U_RoleId,
                        U_DepartmentID = @U_DepartmentID,
                        U_ReportingToId = @U_ReportingToId,
                        Password_hash = @Password_hash,
                        UpdatedDate = GETDATE()
                    WHERE UserId = @UserId";
        
        var parameters = new
        {
            UserId = userId,
            user.Username,
            user.U_Name,
            user.U_Mobile,
            user.U_Email,
            user.U_Address,
            user.U_RoleId,
            user.U_DepartmentID,
            user.U_ReportingToId,
            user.Password_hash
        };

        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = CreateConnection();
        var sql = "SELECT COUNT(1) FROM tblUsers WHERE Username = @Username";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Username = username });
        return count > 0;
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblUsers WHERE UserId = @UserId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { UserId = userId });
        return rowsAffected > 0;
    }
}
