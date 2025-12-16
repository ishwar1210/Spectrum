using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly string _connectionString;

    public ParcelRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<int> CreateAsync(Parcel parcel)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO tblParcel (ParcelBarcode, ParcelCompanyName, UserId, CreatedDate, IsActive, ParcelHandover)
                    VALUES (@ParcelBarcode, @ParcelCompanyName, @UserId, GETDATE(), @IsActive, @ParcelHandover);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

        var parameters = new
        {
            ParcelBarcode = parcel.ParcelBarcode,
            ParcelCompanyName = parcel.ParcelCompanyName,
            UserId = parcel.UserId,
            IsActive = parcel.IsActive,
            ParcelHandover = parcel.ParcelHandover
        };

        var result = await connection.ExecuteScalarAsync<int?>(sql, parameters);
        return result ?? 0;
    }

    public async Task<Parcel?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = @"SELECT ParcelId, ParcelBarcode, ParcelCompanyName, UserId, CreatedDate, UpdatedDate, IsActive, ParcelHandover
                    FROM tblParcel
                    WHERE ParcelId = @Id";
        return await connection.QueryFirstOrDefaultAsync<Parcel>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Parcel>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = @"SELECT ParcelId, ParcelBarcode, ParcelCompanyName, UserId, CreatedDate, UpdatedDate, IsActive, ParcelHandover
                    FROM tblParcel
                    ORDER BY CreatedDate DESC";
        return await connection.QueryAsync<Parcel>(sql);
    }

    public async Task<bool> UpdateAsync(int id, Parcel parcel)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE tblParcel
                    SET ParcelBarcode = @ParcelBarcode,
                        ParcelCompanyName = @ParcelCompanyName,
                        UserId = @UserId,
                        IsActive = @IsActive,
                        ParcelHandover = @ParcelHandover,
                        UpdatedDate = GETDATE()
                    WHERE ParcelId = @Id";

        var parameters = new
        {
            Id = id,
            ParcelBarcode = parcel.ParcelBarcode,
            ParcelCompanyName = parcel.ParcelCompanyName,
            UserId = parcel.UserId,
            IsActive = parcel.IsActive,
            ParcelHandover = parcel.ParcelHandover
        };

        var affectedRows = await connection.ExecuteAsync(sql, parameters);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = @"DELETE FROM tblParcel WHERE ParcelId = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}
