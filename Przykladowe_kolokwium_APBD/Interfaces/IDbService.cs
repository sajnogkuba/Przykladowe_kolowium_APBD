using Kolokwium1_APBD.Models;

namespace Kolokwium1_APBD.Interfaces;

public interface IDbService
{
    Task<List<Prescription>?> GetAllPrescriptions();
    Task<List<Prescription>?> GetPrescriptionsByDoctor(int doctorId);
    Task<Doctor?> GetDoctorByLastName(string? lastName);
    void PutTransaction();
}