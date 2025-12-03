using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class MeetingRoomRepository : IMeetingRoomRepository
{
    private readonly string _connectionString;

    public MeetingRoomRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<MeetingRoom?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblMeetingRoom WHERE MeetingRoomId = @Id";
        return await connection.QueryFirstOrDefaultAsync<MeetingRoom>(sql, new { Id = id });
    }

    public async Task<IEnumerable<MeetingRoom>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblMeetingRoom ORDER BY MeetingRoom_Name";
        return await connection.QueryAsync<MeetingRoom>(sql);
    }

    public async Task<int> CreateAsync(MeetingRoom m)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblMeetingRoom (MeetingRoom_Name, MeetingRoom_Floor, MeetingRoom_Capacity, MeetingRoom_AminitiesId, CreatedDate)
                    VALUES (@MeetingRoom_Name, @MeetingRoom_Floor, @MeetingRoom_Capacity, @MeetingRoom_AminitiesId, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, m);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, MeetingRoom m)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblMeetingRoom SET MeetingRoom_Name = @MeetingRoom_Name, MeetingRoom_Floor = @MeetingRoom_Floor, MeetingRoom_Capacity = @MeetingRoom_Capacity, MeetingRoom_AminitiesId = @MeetingRoom_AminitiesId, UpdatedDate = GETDATE() WHERE MeetingRoomId = @Id";
        var parameters = new
        {
            Id = id,
            m.MeetingRoom_Name,
            m.MeetingRoom_Floor,
            m.MeetingRoom_Capacity,
            m.MeetingRoom_AminitiesId
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblMeetingRoom WHERE MeetingRoomId = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
