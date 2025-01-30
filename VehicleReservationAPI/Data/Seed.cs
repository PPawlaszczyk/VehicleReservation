using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationAPI.CQRS.Vehicles.Commands;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Interfaces;

namespace VehicleReservationAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {

            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var vehicleData = await File.ReadAllTextAsync("Data/VehicleSeed.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            var vehicles = JsonSerializer.Deserialize<List<CreateVehicleCommand>>(vehicleData, options);

            if (users == null) return;

            var roles = new List<AppRole>{
                new() {Name = "Member"},
                new() {Name = "Admin"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName!.ToLower(); ;
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            foreach (var vehicle in vehicles)
            {
                unitOfWork.VehiclesRepository.AddVehicle(vehicle);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                Created = DateTime.UtcNow,
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, ["Admin"]);
        }
    }
}