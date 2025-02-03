using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationAPI.CQRS.Reservations.Commands;
using VehicleReservationAPI.CQRS.Reservations.Queries;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Extensions;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController(IMediator mediator, IMessageProducer messageProducer) : ControllerBase
    {
        [Authorize]
        [HttpPost("create-reservation-vehicle")]
        public async Task<ActionResult<Guid>> CreateReservation([FromBody] CreateReservationDto reservation)
        {
            var reservationCommand = new CreateReservationCommand
            {
                AppUserId = User.GetUserId(),
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                VehicleId = reservation.VehicleId,
            };

            await mediator.Send(reservationCommand);
            await messageProducer.SendingMessage(reservationCommand);
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
        [HttpGet("get-user-all-reservations")]
        public async Task<ActionResult<IEnumerable<UserReservationDto>>> GetUserAllReservations()
        {
            return Ok(await mediator.Send(
                new GetUserAllReservationsQuery 
                { 
                    AppUserId = User.GetUserId() 
                }
                ));            
        }
    }
}