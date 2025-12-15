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

    public async Task<int> CreateAsync(VisitorEntry entry)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblVisitorEntry (VisitorEntry_visitorId, VisitorEntry_Gatepass, VisitorEntry_Vehicletype, VisitorEntry_Vehicleno, VisitorEntry_Date, VisitorEntry_Intime, VisitorEntry_Outtime, VisitorEntry_Userid, VisitorEntryAdmin_isApproval, VisitorEntryuser_isApproval, VisitorEntry_Remark, VisitorEntry_isCanteen, VisitorEntry_isStay, CreatedDate)
                    VALUES (@VisitorEntry_visitorId, @VisitorEntry_Gatepass, @VisitorEntry_Vehicletype, @VisitorEntry_Vehicleno, @VisitorEntry_Date, @VisitorEntry_Intime, @VisitorEntry_Outtime, @VisitorEntry_Userid, @VisitorEntryAdmin_isApproval, @VisitorEntryuser_isApproval, @VisitorEntry_Remark, @VisitorEntry_isCanteen, @VisitorEntry_isStay, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";

        var parameters = new
        {
            VisitorEntry_visitorId = entry.VisitorEntry_visitorId,
            VisitorEntry_Gatepass = entry.VisitorEntry_Gatepass,
            VisitorEntry_Vehicletype = entry.VisitorEntry_Vehicletype,
            VisitorEntry_Vehicleno = entry.VisitorEntry_Vehicleno,
            VisitorEntry_Date = entry.VisitorEntry_Date,
            VisitorEntry_Intime = entry.VisitorEntry_Intime,
            VisitorEntry_Outtime = entry.VisitorEntry_Outtime,
            VisitorEntry_Userid = entry.VisitorEntry_Userid,
            VisitorEntryAdmin_isApproval = entry.VisitorEntryAdmin_isApproval,
            VisitorEntryuser_isApproval = entry.VisitorEntryuser_isApproval,
            VisitorEntry_Remark = entry.VisitorEntry_Remark,
            VisitorEntry_isCanteen = entry.VisitorEntry_isCanteen,
            VisitorEntry_isStay = entry.VisitorEntry_isStay
        };

        var result = await connection.ExecuteScalarAsync<int?>(sql, parameters);
        return result ?? 0;
    }

    public async Task<VisitorEntry?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = @"SELECT * FROM tblVisitorEntry WHERE VisitorEntryID = @Id";
        return await connection.QueryFirstOrDefaultAsync<VisitorEntry>(sql, new { Id = id });
    }

    public async Task<IEnumerable<VisitorEntry>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = @"SELECT * FROM tblVisitorEntry ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<VisitorEntry>(sql);
    }

    public async Task<bool> UpdateAsync(int id, VisitorEntry entry)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblVisitorEntry
                    SET VisitorEntry_visitorId = @VisitorEntry_visitorId,
                        VisitorEntry_Gatepass = @VisitorEntry_Gatepass,
                        VisitorEntry_Vehicletype = @VisitorEntry_Vehicletype,
                        VisitorEntry_Vehicleno = @VisitorEntry_Vehicleno,
                        VisitorEntry_Date = @VisitorEntry_Date,
                        VisitorEntry_Intime = @VisitorEntry_Intime,
                        VisitorEntry_Outtime = @VisitorEntry_Outtime,
                        VisitorEntry_Userid = @VisitorEntry_Userid,
                        VisitorEntryAdmin_isApproval = @VisitorEntryAdmin_isApproval,
                        VisitorEntryuser_isApproval = @VisitorEntryuser_isApproval,
                        VisitorEntry_Remark = @VisitorEntry_Remark,
                        VisitorEntry_isCanteen = @VisitorEntry_isCanteen,
                        VisitorEntry_isStay = @VisitorEntry_isStay,
                        UpdatedDate = GETDATE()
                    WHERE VisitorEntryID = @Id";
        var parameters = new
        {
            Id = id,
            VisitorEntry_visitorId = entry.VisitorEntry_visitorId,
            VisitorEntry_Gatepass = entry.VisitorEntry_Gatepass,
            VisitorEntry_Vehicletype = entry.VisitorEntry_Vehicletype,
            VisitorEntry_Vehicleno = entry.VisitorEntry_Vehicleno,
            VisitorEntry_Date = entry.VisitorEntry_Date,
            VisitorEntry_Intime = entry.VisitorEntry_Intime,
            VisitorEntry_Outtime = entry.VisitorEntry_Outtime,
            VisitorEntry_Userid = entry.VisitorEntry_Userid,
            VisitorEntryAdmin_isApproval = entry.VisitorEntryAdmin_isApproval,
            VisitorEntryuser_isApproval = entry.VisitorEntryuser_isApproval,
            VisitorEntry_Remark = entry.VisitorEntry_Remark,
            VisitorEntry_isCanteen = entry.VisitorEntry_isCanteen,
            VisitorEntry_isStay = entry.VisitorEntry_isStay
        };
        var affectedRows = await connection.ExecuteAsync(sql, parameters);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = @"DELETE FROM tblVisitorEntry WHERE VisitorEntryID = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}
