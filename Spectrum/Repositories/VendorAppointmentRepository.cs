using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class VendorAppointmentRepository : IVendorAppointmentRepository
{
    private readonly string _connectionString;

    public VendorAppointmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<VendorAppointment?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorA_Id, VendorA_VendorID, VendorA_Getpass, VendorA_FromDate, VendorA_ToDate, VendorA_VehicleNO, VendorA_IdProofType, VendorA_IdProofNo, VendorA_UserId, CreatedDate, UpdatedDate FROM tblVendorAppointment WHERE VendorA_Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<VendorAppointment>(sql, new { Id = id });
    }

    public async Task<IEnumerable<VendorAppointment>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorA_Id, VendorA_VendorID, VendorA_Getpass, VendorA_FromDate, VendorA_ToDate, VendorA_VehicleNO, VendorA_IdProofType, VendorA_IdProofNo, VendorA_UserId, CreatedDate, UpdatedDate FROM tblVendorAppointment ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<VendorAppointment>(sql);
    }

    public async Task<int> CreateAsync(VendorAppointment appointment)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblVendorAppointment (VendorA_VendorID, VendorA_Getpass, VendorA_FromDate, VendorA_ToDate, VendorA_VehicleNO, VendorA_IdProofType, VendorA_IdProofNo, VendorA_UserId, CreatedDate)
                    VALUES (@VendorA_VendorID, @VendorA_Getpass, @VendorA_FromDate, @VendorA_ToDate, @VendorA_VehicleNO, @VendorA_IdProofType, @VendorA_IdProofNo, @VendorA_UserId, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, appointment);
    }

    public async Task<bool> UpdateAsync(int id, VendorAppointment appointment)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblVendorAppointment SET VendorA_VendorID = @VendorA_VendorID, VendorA_Getpass = @VendorA_Getpass, VendorA_FromDate = @VendorA_FromDate, VendorA_ToDate = @VendorA_ToDate, VendorA_VehicleNO = @VendorA_VehicleNO, VendorA_IdProofType = @VendorA_IdProofType, VendorA_IdProofNo = @VendorA_IdProofNo, VendorA_UserId = @VendorA_UserId, UpdatedDate = GETDATE() WHERE VendorA_Id = @Id";
        var parameters = new
        {
            Id = id,
            appointment.VendorA_VendorID,
            appointment.VendorA_Getpass,
            appointment.VendorA_FromDate,
            appointment.VendorA_ToDate,
            appointment.VendorA_VehicleNO,
            appointment.VendorA_IdProofType,
            appointment.VendorA_IdProofNo,
            appointment.VendorA_UserId
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblVendorAppointment WHERE VendorA_Id = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
