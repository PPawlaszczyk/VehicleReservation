using FluentValidation;
using VehicleReservationAPI.CQRS.Account.Commands;
using VehicleReservationAPI.CQRS.Reservations.Commands;

namespace VehicleReservationAPI.Validators
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(c => c.StartDate)
                .LessThan(c => c.EndDate)
                .WithMessage("Start date cannot be later than end date or equal to EndDate.");

            RuleFor(c => c.StartDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage(c => $"Start date cannot be earlier than {DateOnly.FromDateTime(DateTime.UtcNow)}.");

            RuleFor(c => c.StartDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(1))
                .WithMessage("Start date cannot be more than 1 year from now.");

            RuleFor(c => c.EndDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(1))
                .WithMessage("End date cannot be more than 1 year from now.");
        }
    }
}