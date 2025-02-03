using MediatR;
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
            await unitOfWork.ReservationRepository.ReturnReservationAsync(command.ReservationId);

            if (await unitOfWork.Complete())
            {
                return;
            }

            throw new InvalidOperationException("Couldn't return vehicle.");
        }
    }
}