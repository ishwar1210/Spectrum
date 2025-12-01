using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class ApprovalStatusRepository : IApprovalStatusRepository
{
    private readonly string _connectionString;

    public ApprovalStatusRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<ApprovalStatus?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM ApprovalStatus WHERE ApprovalStatusId = @Id";
        return await connection.QueryFirstOrDefaultAsync<ApprovalStatus>(sql, new { Id = id });
    }

    public async Task<IEnumerable<ApprovalStatus>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM ApprovalStatus ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<ApprovalStatus>(sql);
    }

    public async Task<int> CreateAsync(ApprovalStatus a)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO ApprovalStatus (ApprovalStatus_Gatepass, ApprovalStatus_VisitorEntryId, ApprovalStatus_TransactionDate, ApprovalStatus_ApprovalDate, ApprovalStatus_ApprovalStatus, ApprovalStatus_Remark, ApprovalStatus_ApprovalPersonRoleID, ApprovalStatus_UserId, CreatedDate)
                    VALUES (@ApprovalStatus_Gatepass, @ApprovalStatus_VisitorEntryId, @ApprovalStatus_TransactionDate, @ApprovalStatus_ApprovalDate, @ApprovalStatus_ApprovalStatus, @ApprovalStatus_Remark, @ApprovalStatus_ApprovalPersonRoleID, @ApprovalStatus_UserId, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, a);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, ApprovalStatus a)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE ApprovalStatus SET ApprovalStatus_Gatepass = @ApprovalStatus_Gatepass, ApprovalStatus_VisitorEntryId = @ApprovalStatus_VisitorEntryId, ApprovalStatus_TransactionDate = @ApprovalStatus_TransactionDate, ApprovalStatus_ApprovalDate = @ApprovalStatus_ApprovalDate, ApprovalStatus_ApprovalStatus = @ApprovalStatus_ApprovalStatus, ApprovalStatus_Remark = @ApprovalStatus_Remark, ApprovalStatus_ApprovalPersonRoleID = @ApprovalStatus_ApprovalPersonRoleID, ApprovalStatus_UserId = @ApprovalStatus_UserId, UpdatedDate = GETDATE() WHERE ApprovalStatusId = @Id";
        var parameters = new
        {
            Id = id,
            a.ApprovalStatus_Gatepass,
            a.ApprovalStatus_VisitorEntryId,
            a.ApprovalStatus_TransactionDate,
            a.ApprovalStatus_ApprovalDate,
            a.ApprovalStatus_ApprovalStatus,
            a.ApprovalStatus_Remark,
            a.ApprovalStatus_ApprovalPersonRoleID,
            a.ApprovalStatus_UserId
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM ApprovalStatus WHERE ApprovalStatusId = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
