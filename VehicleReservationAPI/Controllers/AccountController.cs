using Microsoft.AspNetCore.Mvc;
using VehicleReservationAPI.CQRS.Account.Commands;
using VehicleReservationAPI.DTOs;
using MediatR;
using VehicleReservationAPI.Vehicles.Queries;

namespace VehicleReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterCommand register)
        {
            return Ok(await mediator.Send(register));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginQuery login)
        {
            return Ok(await mediator.Send(login));
        }
    }
}