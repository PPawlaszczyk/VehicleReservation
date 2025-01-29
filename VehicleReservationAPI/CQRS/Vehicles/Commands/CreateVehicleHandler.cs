using MediatR;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Extensions;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.Validators;

namespace VehicleReservationAPI.CQRS.Vehicles.Commands
{
    public record CreateVehicleCommand : IRequest<Guid>
    {
        public required string Name { get; init; }
        public required VehicleType Type { get; init; }
        public required string Mark { get; init; }
        public required int Seats { get; init; }
        public required string Fuel { get; init; }
        public required int Year { get; init; }
        public required double Cost { get; init; }
        public required string RegistrationNumber { get; init; }
    }

    public class CreateVehicleHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateVehicleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateVehicleCommandValidator();
            validator.ThrowIfInvalid(command);

            if (await unitOfWork.VehiclesRepository.IsVehicleRegistrationExistsAsync(command.RegistrationNumber))
            {
                throw new InvalidOperationException("This registration number already exists.");
            }

            var response = unitOfWork.VehiclesRepository.AddVehicle(command);

            if (await unitOfWork.Complete())
            {
                return response.Id;
            }

            throw new InvalidOperationException("Couldn't add new car.");
        }
    }
}