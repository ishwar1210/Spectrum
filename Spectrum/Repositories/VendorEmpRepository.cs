using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class VendorEmpRepository : IVendorEmpRepository
{
    private readonly string _connectionString;

    public VendorEmpRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<VendorEmp?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorEmpId, VendorEmp_VendorID, VendorEmp_Name, VendorEmp_IDProofType, VendorEmp_IDProofNo, VendorEmp_mobile, VendorEmp_VenderAID, CreatedDate, UpdatedDate FROM tblVendorEmp WHERE VendorEmpId = @Id";
        return await connection.QueryFirstOrDefaultAsync<VendorEmp>(sql, new { Id = id });
    }

    public async Task<IEnumerable<VendorEmp>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorEmpId, VendorEmp_VendorID, VendorEmp_Name, VendorEmp_IDProofType, VendorEmp_IDProofNo, VendorEmp_mobile, VendorEmp_VenderAID, CreatedDate, UpdatedDate FROM tblVendorEmp ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<VendorEmp>(sql);
    }

    public async Task<int> CreateAsync(VendorEmp emp)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblVendorEmp (VendorEmp_VendorID, VendorEmp_Name, VendorEmp_IDProofType, VendorEmp_IDProofNo, VendorEmp_mobile, VendorEmp_VenderAID, CreatedDate)
                    VALUES (@VendorEmp_VendorID, @VendorEmp_Name, @VendorEmp_IDProofType, @VendorEmp_IDProofNo, @VendorEmp_mobile, @VendorEmp_VenderAID, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, emp);
    }

    public async Task<bool> UpdateAsync(int id, VendorEmp emp)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblVendorEmp SET VendorEmp_VendorID = @VendorEmp_VendorID, VendorEmp_Name = @VendorEmp_Name, VendorEmp_IDProofType = @VendorEmp_IDProofType, VendorEmp_IDProofNo = @VendorEmp_IDProofNo, VendorEmp_mobile = @VendorEmp_mobile, VendorEmp_VenderAID = @VendorEmp_VenderAID, UpdatedDate = GETDATE() WHERE VendorEmpId = @Id";
        var parameters = new
        {
            Id = id,
            emp.VendorEmp_VendorID,
            emp.VendorEmp_Name,
            emp.VendorEmp_IDProofType,
            emp.VendorEmp_IDProofNo,
            emp.VendorEmp_mobile,
            emp.VendorEmp_VenderAID
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblVendorEmp WHERE VendorEmpId = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
