namespace Kolokwium1_APBD.DTOs;

public class PrescriptionDTO
{
    public record CreatePrescriptionDto(
        int IdPrescription,
        DateTime Date,
        DateTime DueDate,
        int IdPatient,
        int IdDoctor);
}