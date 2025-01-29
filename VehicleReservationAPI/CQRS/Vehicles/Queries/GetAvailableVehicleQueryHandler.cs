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
        public async Task<IEnumerable<GetAvailableVehiclesDto>> Handle(GetAvailableVehicleQuery command, CancellationToken cancellationToken)
        {
            var validator = new GetAvailableVehiclesCommandValidator();
            validator.ThrowIfInvalid(command);

            var response = await unitOfWork.VehiclesRepository.GetAvailbleVehiclesAsync
                (
                startDate: command.StartDate,
                endDate: command.EndDate,
                type: command.Type
                ) ?? [];

            return response;
        }
    }
}