using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class VisitorEntryRepository : IVisitorEntryRepository
{
    private readonly string _connectionString;

    public VisitorEntryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<VisitorEntry?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblVisitorEntry WHERE VisitorEntryID = @Id";
        return await connection.QueryFirstOrDefaultAsync<VisitorEntry>(sql, new { Id = id });
    }

    public async Task<IEnumerable<VisitorEntry>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblVisitorEntry ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<VisitorEntry>(sql);
    }

    public async Task<int> CreateAsync(VisitorEntry entry)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblVisitorEntry (VisitorEntry_visitorId, VisitorEntry_Gatepass, VisitorEntry_Vehicletype, VisitorEntry_Vehicleno, VisitorEntry_Date, VisitorEntry_Intime, VisitorEntry_Outtime, VisitorEntry_Userid, VisitorEntry_isApproval, VisitorEntry_Remark, VisitorEntry_isCanteen, VisitorEntry_isStay, CreatedDate)
                    VALUES (@VisitorEntry_visitorId, @VisitorEntry_Gatepass, @VisitorEntry_Vehicletype, @VisitorEntry_Vehicleno, @VisitorEntry_Date, @VisitorEntry_Intime, @VisitorEntry_Outtime, @VisitorEntry_Userid, @VisitorEntry_isApproval, @VisitorEntry_Remark, @VisitorEntry_isCanteen, @VisitorEntry_isStay, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, entry);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, VisitorEntry entry)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblVisitorEntry SET VisitorEntry_visitorId = @VisitorEntry_visitorId, VisitorEntry_Gatepass = @VisitorEntry_Gatepass, VisitorEntry_Vehicletype = @VisitorEntry_Vehicletype, VisitorEntry_Vehicleno = @VisitorEntry_Vehicleno, VisitorEntry_Date = @VisitorEntry_Date, VisitorEntry_Intime = @VisitorEntry_Intime, VisitorEntry_Outtime = @VisitorEntry_Outtime, VisitorEntry_Userid = @VisitorEntry_Userid, VisitorEntry_isApproval = @VisitorEntry_isApproval, VisitorEntry_Remark = @VisitorEntry_Remark, VisitorEntry_isCanteen = @VisitorEntry_isCanteen, VisitorEntry_isStay = @VisitorEntry_isStay, UpdatedDate = GETDATE() WHERE VisitorEntryID = @Id";
        var parameters = new
        {
            Id = id,
            entry.VisitorEntry_visitorId,
            entry.VisitorEntry_Gatepass,
            entry.VisitorEntry_Vehicletype,
            entry.VisitorEntry_Vehicleno,
            entry.VisitorEntry_Date,
            entry.VisitorEntry_Intime,
            entry.VisitorEntry_Outtime,
            entry.VisitorEntry_Userid,
            entry.VisitorEntry_isApproval,
            entry.VisitorEntry_Remark,
            entry.VisitorEntry_isCanteen,
            entry.VisitorEntry_isStay
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblVisitorEntry WHERE VisitorEntryID = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
