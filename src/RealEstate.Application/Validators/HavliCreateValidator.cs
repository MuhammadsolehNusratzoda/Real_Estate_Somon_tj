using FluentValidation;
using RealEstate.Application.DTOs.Property;

namespace RealEstate.Application.Validators;

public class HavliCreateValidator : AbstractValidator<HavliCreateDto>
{
    public HavliCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Area).GreaterThan(0);
        RuleFor(x => x.LandArea).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Rooms).GreaterThan(0);
        RuleFor(x => x.Region).NotEmpty();
        RuleFor(x => x.District).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
    }
}
