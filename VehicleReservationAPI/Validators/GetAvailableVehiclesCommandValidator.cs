using FluentValidation;
using VehicleReservationAPI.CQRS.Vehicles.Queries;
using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.Validators
{
    public class GetAvailableVehiclesCommandValidator : AbstractValidator<GetAvailableVehicleQuery>
    {
        public GetAvailableVehiclesCommandValidator()
        {
            RuleFor(c => c.StartDate)
                .LessThan(c => c.EndDate)
                .WithMessage("Start date cannot be later than end date.");

            RuleFor(c => c.StartDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage(c => $"Start date cannot be earlier than {DateOnly.FromDateTime(DateTime.UtcNow)}.");

            RuleFor(c => c.StartDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(1))
                .WithMessage("Start date cannot be more than 1 year from now.");

            RuleFor(c => c.EndDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(1))
                .WithMessage("End date cannot be more than 1 year from now.");

            RuleFor(c => c.Type)
                .NotEqual(VehicleType.None)
                .WithMessage("Type cannot be 'None'.");
        }
    }
}