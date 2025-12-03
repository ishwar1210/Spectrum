using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class RoomBookingRepository : IRoomBookingRepository
{
    private readonly string _connectionString;

    public RoomBookingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<RoomBooking?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblRoomBooking WHERE RoomBookingID = @Id";
        return await connection.QueryFirstOrDefaultAsync<RoomBooking>(sql, new { Id = id });
    }

    public async Task<IEnumerable<RoomBooking>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblRoomBooking ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<RoomBooking>(sql);
    }

    public async Task<int> CreateAsync(RoomBooking rb)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblRoomBooking (RoomBooking_MeetingroomId, RoomBooking_UserID, RoomBooking_VisitorID, RoomBooking_MeetingDate, RoomBooking_Starttime, RoomBooking_Endtime, CreatedDate)
                    VALUES (@RoomBooking_MeetingroomId, @RoomBooking_UserID, @RoomBooking_VisitorID, @RoomBooking_MeetingDate, @RoomBooking_Starttime, @RoomBooking_Endtime, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, rb);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, RoomBooking rb)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblRoomBooking SET RoomBooking_MeetingroomId = @RoomBooking_MeetingroomId, RoomBooking_UserID = @RoomBooking_UserID, RoomBooking_VisitorID = @RoomBooking_VisitorID, RoomBooking_MeetingDate = @RoomBooking_MeetingDate, RoomBooking_Starttime = @RoomBooking_Starttime, RoomBooking_Endtime = @RoomBooking_Endtime, UpdatedDate = GETDATE() WHERE RoomBookingID = @Id";
        var parameters = new
        {
            Id = id,
            rb.RoomBooking_MeetingroomId,
            rb.RoomBooking_UserID,
            rb.RoomBooking_VisitorID,
            rb.RoomBooking_MeetingDate,
            rb.RoomBooking_Starttime,
            rb.RoomBooking_Endtime
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblRoomBooking WHERE RoomBookingID = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
