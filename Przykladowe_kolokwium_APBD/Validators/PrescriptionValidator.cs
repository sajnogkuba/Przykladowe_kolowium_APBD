using FluentValidation;
using Kolokwium1_APBD.DTOs;

namespace Przykladowe_kolokwium_APBD.Validators;

public class PrescriptionValidator : AbstractValidator<PrescriptionDTO>
{
    public PrescriptionValidator()
    {
        RuleFor(e => e)
    }
}