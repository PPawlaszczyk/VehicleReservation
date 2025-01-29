using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController(IMediator mediator, IMessageProducer messageProducer) : ControllerBase
    {
        [Authorize]
        [HttpPost("create-reservation-vehicle")]
        public async Task<ActionResult<Guid>> CreateReservation([FromBody] CreateReservationCommand reservation)
        {
            await mediator.Send(reservation);

            await messageProducer.SendingMessage(reservation);
            return Ok();
        }

        [Authorize]
        [HttpPost("return-reserved-vehicle")]
        public async Task<ActionResult> ReturnReservedVehicle([FromBody] ReturnReservedVehicleCommand reservation)
        {
            await mediator.Send(reservation);
            return Ok();
        }

        [Authorize]
        [HttpPut("update-reserved-vehicle")]
        public async Task<ActionResult> UpdateReservedVehicle([FromBody] ReturnReservedVehicleCommand reservation)
        {
            throw new NotImplementedException();

        }

        [Authorize]
        [HttpPost("Cancel-reserved-vehicle")]
        public async Task<ActionResult> CancelReservedVehicle([FromBody] ReturnReservedVehicleCommand reservation)
        {
            throw new NotImplementedException();

        }
    }
}