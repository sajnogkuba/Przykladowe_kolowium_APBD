using FluentValidation;
using Kolokwium1_APBD.DTOs;

namespace Przykladowe_kolokwium_APBD.Validators;

public class PrescriptionValidator : AbstractValidator<CreatePrescriptionDto>
{
    public PrescriptionValidator()
    {
        RuleFor(e => e.IdPrescription).NotNull();
        RuleFor(e => e.Date).NotNull();
        RuleFor(e => e.DueDate).NotNull();
        RuleFor(e => e.IdPatient).NotNull();
        RuleFor(e => e.IdDoctor).NotNull();
    }
}