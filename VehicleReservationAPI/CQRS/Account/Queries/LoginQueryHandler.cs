using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Vehicles.Queries
{

    public record LoginQuery : IRequest<UserDto>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService) : IRequestHandler<LoginQuery, UserDto>
    {
        public async Task<UserDto> Handle(LoginQuery command, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == command.Username.ToUpper());

            if (user == null || user.UserName == null)
            {
                throw new InvalidOperationException("Invalid username");
            }

            var result = await userManager.CheckPasswordAsync(user, command.Password);

            if (!result)
            {
                throw new InvalidOperationException("Unauthorized");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
            };
        }
    }   
}