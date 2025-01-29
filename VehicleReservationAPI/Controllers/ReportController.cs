using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.CQRS.Vehicles.Queries;

namespace VehicleReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-report-reservations")]
        public async Task<ActionResult<IEnumerable<GetAvailableVehicleQueryHandler>>> CreateReportReservations([FromBody] CreateVehicleCommand vehicle)
        {
            throw new NotImplementedException();
        }
    }
}