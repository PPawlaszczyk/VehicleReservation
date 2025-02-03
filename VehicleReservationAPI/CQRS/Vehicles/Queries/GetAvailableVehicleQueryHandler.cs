using MediatR;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Enums;
using VehicleReservationAPI.Extensions;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.Validators;

namespace VehicleReservationAPI.CQRS.Vehicles.Queries
{
    public record GetAvailableVehicleQuery : IRequest<IEnumerable<GetAvailableVehiclesDto>>
    {
        public required DateOnly EndDate { get; init; }
        public required DateOnly StartDate { get; init; }
        public required VehicleType Type { get; init; }
    }

    public class GetAvailableVehicleQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAvailableVehicleQuery, IEnumerable<GetAvailableVehiclesDto>>
    {
        public async Task<IEnumerable<GetAvailableVehiclesDto>> Handle(GetAvailableVehicleQuery query, CancellationToken cancellationToken)
        {
            var validator = new GetAvailableVehiclesCommandValidator();
            validator.ThrowIfInvalid(query);

            var response = await unitOfWork.VehiclesRepository.GetAvailbleVehiclesAsync
                (
                startDate: query.StartDate,
                endDate: query.EndDate,
                type: query.Type
                ) ?? [];

            return response;
        }
    }
}