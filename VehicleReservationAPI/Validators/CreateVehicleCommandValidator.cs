using FluentValidation;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.Validators
{
    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(v => v.Name.Trim())
                .NotEmpty().WithMessage("Name is required.")
                .NotNull().WithMessage("Name cannot be null.");

            RuleFor(v => v.Type)
                .IsInEnum().WithMessage("Invalid vehicle type.")
                .NotEqual(VehicleType.None).WithMessage("Invalid vehicle type.");

            RuleFor(v => v.Mark.Trim())
                .NotEmpty().WithMessage("Mark is required.")
                .NotNull().WithMessage("Mark cannot be null.");

            RuleFor(v => v.Seats)
                .GreaterThan(0).WithMessage("Seats must be greater than 0.");

            RuleFor(v => v.Fuel.Trim())
                .NotEmpty().WithMessage("Fuel is required.")
                .NotNull().WithMessage("Fuel cannot be null.");

            RuleFor(v => v.Year)
                 .InclusiveBetween(1886, DateTime.Now.Year)
                 .WithMessage($"Year must be between 1886 and {DateTime.Now.Year}.");

            RuleFor(v => v.Cost)
                    .GreaterThan(0).WithMessage("Cost cannot be less than 0.")
                    .LessThanOrEqualTo(1_000_000_000).WithMessage($"Cost cannot exceed 1_000_000_000.");

            RuleFor(v => v.RegistrationNumber.Trim())
                .NotEmpty().WithMessage("Registration Number is required.")
                .NotNull().WithMessage("Fuel cannot be null.");
        }
    }
}