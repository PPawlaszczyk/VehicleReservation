using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.CQRS.Vehicles.Queries;
using VehicleReservationAPI.Enums;

namespace VehicleReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IMediator mediator) : ControllerBase
    {
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<GetAvailableVehicleQueryHandler>>> GetAvailableVehicles([FromQuery] DateOnly startDate, DateOnly endDate, VehicleType type)
        {
            return Ok(await mediator.Send(new GetAvailableVehicleQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                Type = type
            }));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create")]
        public async Task<ActionResult<IEnumerable<GetAvailableVehicleQueryHandler>>> CreateVehicle([FromBody] CreateVehicleCommand vehicle)
        {
            return Ok(await mediator.Send(vehicle));
        }
    }
}