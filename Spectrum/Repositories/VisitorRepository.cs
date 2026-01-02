using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class VisitorRepository : IVisitorRepository
{
    private readonly string _connectionString;

    public VisitorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<Visitor?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblVisitor WHERE VisitorId = @Id";
        return await connection.QueryFirstOrDefaultAsync<Visitor>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Visitor>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM tblVisitor ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<Visitor>(sql);
    }

    public async Task<int> CreateAsync(Visitor visitor)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblVisitor (Visitor_Name, Visitor_mobile, Visitor_Address, Visitor_CompanyName, Visitor_Idprooftype, Visitor_idproofno, Visitor_Fingerprint1, Visitor_Fingerprint2, Visitor_Carrymateriallist, Visitor_Materialbarcode, Visitor_OTP, Visitor_Onofvisit, Visitor_image, Visitor_isApproval, Visitor_isBlock, Visitor_Blockreason, Visitor_Unblockreason, CreatedDate)
                    VALUES (@Visitor_Name, @Visitor_mobile, @Visitor_Address, @Visitor_CompanyName, @Visitor_Idprooftype, @Visitor_idproofno, @Visitor_Fingerprint1, @Visitor_Fingerprint2, @Visitor_Carrymateriallist, @Visitor_Materialbarcode, @Visitor_OTP, @Visitor_Onofvisit, @Visitor_image, @Visitor_isApproval, @Visitor_isBlock, @Visitor_Blockreason, @Visitor_Unblockreason, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        var result = await connection.ExecuteScalarAsync<int?>(sql, visitor);
        return result ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, Visitor visitor)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblVisitor SET Visitor_Name = @Visitor_Name, Visitor_mobile = @Visitor_mobile, Visitor_Address = @Visitor_Address, Visitor_CompanyName = @Visitor_CompanyName, Visitor_Idprooftype = @Visitor_Idprooftype, Visitor_idproofno = @Visitor_idproofno, Visitor_Fingerprint1 = @Visitor_Fingerprint1, Visitor_Fingerprint2 = @Visitor_Fingerprint2, Visitor_Carrymateriallist = @Visitor_Carrymateriallist, Visitor_Materialbarcode = @Visitor_Materialbarcode, Visitor_OTP = @Visitor_OTP, Visitor_Onofvisit = @Visitor_Onofvisit, Visitor_image = @Visitor_image, Visitor_isApproval = @Visitor_isApproval, Visitor_isBlock = @Visitor_isBlock, Visitor_Blockreason = @Visitor_Blockreason, Visitor_Unblockreason = @Visitor_Unblockreason, UpdatedDate = GETDATE() WHERE VisitorId = @Id";
        var parameters = new
        {
            Id = id,
            visitor.Visitor_Name,
            visitor.Visitor_mobile,
            visitor.Visitor_Address,
            visitor.Visitor_CompanyName,
            visitor.Visitor_Idprooftype,
            visitor.Visitor_idproofno,
            visitor.Visitor_Fingerprint1,
            visitor.Visitor_Fingerprint2,
            visitor.Visitor_Carrymateriallist,
            visitor.Visitor_Materialbarcode,
            visitor.Visitor_OTP,
            visitor.Visitor_Onofvisit,
            visitor.Visitor_image,
            visitor.Visitor_isApproval,
            visitor.Visitor_isBlock,
            visitor.Visitor_Blockreason,
            visitor.Visitor_Unblockreason
        };
        var rows = await connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM tblVisitor WHERE VisitorId = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
