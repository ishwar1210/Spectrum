using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectrum.Models;

namespace Spectrum.Repositories;

public class EmailConfigRepository
{
    private readonly string _connectionString;

    public EmailConfigRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<EmailConfig?> GetActiveConfigAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT TOP 1 CompanyEmailId, CompanyName, EmailAddress, EmailPassword, SmtpHost, SmtpPort, EnableSSL, IsActive FROM tblCompanyEmailConfig WHERE IsActive = 1";
        return await connection.QueryFirstOrDefaultAsync<EmailConfig>(sql);
    }
}
