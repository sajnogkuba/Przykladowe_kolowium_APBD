using Kolokwium1_APBD.Interfaces;

namespace Przykladowe_kolokwium_APBD.Endpoints;

public static class PrescriptionEndpoints
{
    public static void RegisterPrescriptionEndpoints(this WebApplication app)
    {
        var prescription = app.MapGroup("prescriptions");
        prescription.MapGet("", GetPrescriptions);
        prescription.MapGet("TEST", GetPrescriptions);
        prescription.MapPut("TEST", PutTransaction);
    }

    private static async Task PutTransaction(IConfiguration configuration, IDbService dbcontext)
    {
        dbcontext.PutTransaction();
    }

    private static async Task<IResult> GetPrescriptions(IConfiguration configuration, IDbService db, string? doctorLastName)
    {
        if (doctorLastName is null)
        {
            var result = await db.GetAllPrescriptions();
            if (result is null)
            {
                Results.NotFound("There isn't  any prescriptions");
            }

            return Results.Ok(result);
        }
        var doctor = await db.GetDoctorByLastName(doctorLastName);
        if (doctor is null)
        {
            return Results.NotFound($"Not Found doctor with Last name {doctorLastName}");
        }

        var result2 = db.GetPrescriptionsByDoctor(doctor.IdDoctor);
        return Results.Ok(result2);
    }
}