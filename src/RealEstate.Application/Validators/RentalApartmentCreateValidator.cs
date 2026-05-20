using FluentValidation;
using RealEstate.Application.DTOs.Property;

namespace RealEstate.Application.Validators;

public class RentalApartmentCreateValidator : AbstractValidator<RentalApartmentCreateDto>
{
    public RentalApartmentCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MonthlyRent).GreaterThan(0);
        RuleFor(x => x.Area).GreaterThan(0);
        RuleFor(x => x.Rooms).GreaterThan(0);
        RuleFor(x => x.Floor).GreaterThan(0);
        RuleFor(x => x.TotalFloors).GreaterThan(0);
        RuleFor(x => x.MinRentalMonths).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Entrance).Must(e => new[] { "A", "B", "C", "D" }.Contains(e))
            .When(x => !string.IsNullOrEmpty(x.Entrance))
            .WithMessage("Entrance must be A, B, C, or D");
        RuleFor(x => x.Region).NotEmpty();
        RuleFor(x => x.District).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
    }
}
