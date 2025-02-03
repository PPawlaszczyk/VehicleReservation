using MediatR;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.CQRS.Reservations.Queries
{
    public record GetUserAllReservationsQuery : IRequest<IEnumerable<UserReservationDto>>
    {
        public required Guid AppUserId { get; init; }
    }

    public class GetUserAllReservationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserAllReservationsQuery, IEnumerable<UserReservationDto>>
    {
        public async Task<IEnumerable<UserReservationDto>> Handle(GetUserAllReservationsQuery query, CancellationToken cancellationToken)
        {

            return await unitOfWork.ReservationRepository.GetCurrentReservationsByUserAsync(query.AppUserId);
        }
    }
}