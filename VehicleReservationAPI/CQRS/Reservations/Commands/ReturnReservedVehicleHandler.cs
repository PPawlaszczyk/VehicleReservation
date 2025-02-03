using MediatR;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.CQRS.Reservations.Commands
{
    public record ReturnReservedVehicleCommand : IRequest
    {
        public required Guid ReservationId { get; init; }
    }

    public class ReturnReservedVehicleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReturnReservedVehicleCommand>
    {
        public async Task Handle(ReturnReservedVehicleCommand command, CancellationToken cancellationToken)
        {
            unitOfWork.ReservationRepository.ReturnReservation(command.ReservationId);

            if (await unitOfWork.Complete())
            {
                return;
            }

            throw new InvalidOperationException("Couldn't return vehicle.");
        }
    }
}