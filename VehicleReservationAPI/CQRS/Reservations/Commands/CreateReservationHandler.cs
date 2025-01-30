using MediatR;
using VehicleReservationAPI.Extensions;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.Validators;

namespace VehicleReservationAPI.CQRS.Reservations.Commands
{
    public record CreateReservationCommand : IRequest<Guid>
    {
        public required DateOnly EndDate { get; init; }
        public required DateOnly StartDate { get; init; }
        public required Guid VehicleId { get; init; }
        public required Guid AppUserId { get; init; }
    }

    public class CreateReservationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateReservationCommand, Guid>
    {
        public async Task<Guid> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateReservationCommandValidator();
            validator.ThrowIfInvalid(command);

            var vehicle = await unitOfWork.VehiclesRepository.GetVehicleByIdAsync(command.VehicleId);

            if (vehicle == null || !vehicle.IsAvailable)
            {
                throw new InvalidOperationException("Vehicle is not available or doesnt exists");
            }

            var isReservation = await unitOfWork.ReservationRepository.IsVehicleReservatedAsync
                (
                startDate: command.StartDate,
                endDate: command.EndDate,
                vehicleid: command.VehicleId
                );

            if (isReservation)
            {
                throw new InvalidOperationException("Cannot make reservation in this time");
            }

            var response = unitOfWork.ReservationRepository.AddVehicleReservation(command); 

            if (await unitOfWork.Complete())
            {
                return response.Id;
            }

            throw new InvalidOperationException("Couldn't add new reservation.");
        }
    }
}