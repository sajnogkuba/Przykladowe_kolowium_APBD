using System.Data;
using System.Data.SqlClient;
using Kolokwium1_APBD.Interfaces;
using Kolokwium1_APBD.Models;

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

    public async Task<List<Prescription>> GetAllPrescriptions()
    {
        var prescriptions = new List<Prescription>();
        await using var connection = await GetConnection();
        var command = new SqlCommand("SELECT * FROM Prescription", connection);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var prescription = new Prescription()
            {
                IdPrescription = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                DueDate = reader.GetDateTime(2),
                IdPatient = reader.GetInt32(3),
                IdDoctor = reader.GetInt32(4)
            };
            prescriptions.Add(prescription);
        }

        return prescriptions.Count == 0 ? null : prescriptions;
    }

    public async Task<List<Prescription>?> GetPrescriptionsByDoctor(int doctorId)
    {
        var prescriptions = new List<Prescription>();
        await using var connection = await GetConnection();
        var command = new SqlCommand(@"SELECT * FROM Prescription WHERE Doctor_IdDoctor = @idDoctor", connection);
        command.Parameters.AddWithValue("@idDoctor", doctorId);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var prescription = new Prescription()
            {
                IdPrescription = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                DueDate = reader.GetDateTime(2),
                IdPatient = reader.GetInt32(3),
                IdDoctor = reader.GetInt32(4)
            };
            prescriptions.Add(prescription);
        }

        return prescriptions.Count == 0 ? null : prescriptions;
    }

    public async Task<Doctor?> GetDoctorByLastName(string? lastName)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(@"SELECT * FROM Doctor WHERE LastName = @lastName", connection);
        command.Parameters.AddWithValue("@lastName", lastName);
        var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Doctor()
        {
            IdDoctor = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            Email = reader.GetString(3),
            LastName = reader.GetString(2)
        };
    }

    public async void  PutTransaction()
    {
        await using var connection = await GetConnection();
        await using var transaction = connection.BeginTransaction();
        try
        {
            // Command for the first table
            var command1 = new SqlCommand("INSERT INTO master.dbo.Doctor (IdDoctor, FirstName, LastName, Email)\nVALUES (3, N'TEST3', N'TEST3', N'TEST3');", connection, transaction);

            // Execute the first command
            await command1.ExecuteNonQueryAsync();

            // Command for the second table
            var command2 = new SqlCommand("INSERT INTO master.dbo.Patient (IdPatient, FirstName, LastName, Birthdate)\nVALUES (2, 2, 2, 2);", connection, transaction);

            // Execute the second command
            await command2.ExecuteNonQueryAsync();

            // Commit the transaction if all commands are successful
            transaction.Commit();
            
        }
        catch (Exception ex)
        {
            // Rollback the transaction if any command fails
            transaction.Rollback();
            Console.WriteLine("Error occurred: " + ex.Message);
        }

    }


    // using (var connection = new SqlConnection(connectionString))
    // {
    //     connection.Open();
    //
    //     using (var transaction = connection.BeginTransaction())
    //     {
    //         try
    //         {
    //             var command = connection.CreateCommand();
    //             command.Transaction = transaction;
    //
    //             command.CommandText = "INSERT INTO SomeTable (Column1) VALUES ('Value1')";
    //             command.ExecuteNonQuery();
    //
    //             command.CommandText = "INSERT INTO AnotherTable (Column2) VALUES ('Value2')";
    //             command.ExecuteNonQuery();
    //
    //             // Commit transaction if all commands succeed
    //             transaction.Commit();
    //         }
    //         catch (Exception ex)
    //         {
    //             // Rollback transaction if any command fails
    //             transaction.Rollback();
    //             Console.WriteLine("Error occurred: " + ex.Message);
    //         }
    //     }
    
}