using MediatR;
using Microsoft.Identity.Client;
using VehicleReservationAPI.Extensions;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.Validators;

namespace VehicleReservationAPI.CQRS.Reservations.Commands
{
    public record CancelReservationCommand : IRequest
    {
        public required Guid ReservationId { get; init; }
    }

    public class CancelReservationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CancelReservationCommand>
    {
        public async Task Handle(CancelReservationCommand command, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Couldn't add new reservation.");
        }
    }
}