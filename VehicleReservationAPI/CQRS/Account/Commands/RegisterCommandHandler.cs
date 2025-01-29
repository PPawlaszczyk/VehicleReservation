using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.DTOs;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.CQRS.Account.Commands
{
    public record RegisterCommand : IRequest<UserDto>
    {
        public required string Username { get; set; } 
        public required string Password { get; set; }
    }

    public class RegisterCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService) : IRequestHandler<RegisterCommand, UserDto>
    {
        public async Task<UserDto> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            if (await UserExsists(command.Username))
            {
                throw new InvalidOperationException("Username is taken");
            }

            AppUser user = new AppUser
            {
                Created = DateTime.UtcNow,
                UserName = command.Username.ToLower(),
                PasswordHash = command.Password
            };

            var result = await userManager.CreateAsync(user, command.Password);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Username = user.UserName,
                    Token = await tokenService.CreateToken(user),
                };
            }
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errorMessages);
        }

        private async Task<bool> UserExsists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
        }
    }
}