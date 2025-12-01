using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class VendorRepository : IVendorRepository
{
    private readonly string _connectionString;

    public VendorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Vendor?> GetByIdAsync(int vendorId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorID, VendorCode, Vendor_Name AS VendorName, Vendor_mobile AS VendorMobile, IDProoftype AS IDProofType, IDProof, Vendor_Address AS VendorAddress, Company, CreatedDate, UpdatedDate, IsActive FROM tblVendors WHERE VendorID = @VendorID";
        return await connection.QueryFirstOrDefaultAsync<Vendor>(sql, new { VendorID = vendorId });
    }

    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorID, VendorCode, Vendor_Name AS VendorName, Vendor_mobile AS VendorMobile, IDProoftype AS IDProofType, IDProof, Vendor_Address AS VendorAddress, Company, CreatedDate, UpdatedDate, IsActive FROM tblVendors ORDER BY Vendor_Name";
        return await connection.QueryAsync<Vendor>(sql);
    }

    public async Task<IEnumerable<Vendor>> GetActiveAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT VendorID, VendorCode, Vendor_Name AS VendorName, Vendor_mobile AS VendorMobile, IDProoftype AS IDProofType, IDProof, Vendor_Address AS VendorAddress, Company, CreatedDate, UpdatedDate, IsActive FROM tblVendors WHERE IsActive = 1 ORDER BY Vendor_Name";
        return await connection.QueryAsync<Vendor>(sql);
    }

    public async Task<int> CreateAsync(Vendor vendor)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblVendors (VendorCode, Vendor_Name, Vendor_mobile, IDProoftype, IDProof, Vendor_Address, Company, CreatedDate, IsActive)
                    VALUES (@VendorCode, @VendorName, @VendorMobile, @IDProofType, @IDProof, @VendorAddress, @Company, GETDATE(), @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, vendor);
    }

    public async Task<bool> UpdateAsync(int vendorId, Vendor vendor)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblVendors SET VendorCode = @VendorCode, Vendor_Name = @VendorName, Vendor_mobile = @VendorMobile, IDProoftype = @IDProofType, IDProof = @IDProof, Vendor_Address = @VendorAddress, Company = @Company, IsActive = @IsActive, UpdatedDate = GETDATE() WHERE VendorID = @VendorID";
        var parameters = new
        {
            VendorID = vendorId,
            vendor.VendorCode,
            vendor.VendorName,
            vendor.VendorMobile,
            vendor.IDProofType,
            vendor.IDProof,
            vendor.VendorAddress,
            vendor.Company,
            vendor.IsActive
        };
        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int vendorId)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblVendors WHERE VendorID = @VendorID";
        var rowsAffected = await connection.ExecuteAsync(sql, new { VendorID = vendorId });
        return rowsAffected > 0;
    }

    public async Task<bool> VendorCodeExistsAsync(string vendorCode, int? excludeVendorId = null)
    {
        using var connection = CreateConnection();
        var sql = excludeVendorId.HasValue
            ? "SELECT COUNT(1) FROM tblVendors WHERE VendorCode = @VendorCode AND VendorID != @ExcludeVendorID"
            : "SELECT COUNT(1) FROM tblVendors WHERE VendorCode = @VendorCode";

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            VendorCode = vendorCode,
            ExcludeVendorID = excludeVendorId
        });
        return count > 0;
    }
}
