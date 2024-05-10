using System.Data;
using System.Data.SqlClient;
using Kolokwium1_APBD.Interfaces;

namespace Kolokwium1_APBD.Services;

public class DbService(IConfiguration configuration) : IDbService
{
    private async Task<SqlConnection> GetConnection()
    {
        var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        return connection;
    }
}