using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VehicleReservationAPI.Data;
using VehicleReservationAPI.Entities;
using VehicleReservationAPI.Extensions;
using VehicleReservationAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
builder.Services.AddHealthChecks();

var app = builder.Build();

//app.MapHub<ReservationHub>("/reservationHub");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

        options.RoutePrefix = "swagger";
    });
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200"));
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.MapHub<PresenceHub>("hubs/message");


app.MapControllers();

using var scope = app.Services.CreateScope();
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        await context.Database.MigrateAsync();

        await Seed.SeedUsers(userManager, roleManager);
        await context.SaveChangesAsync();

    }
    catch (System.Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
    }
}

app.MapHealthChecks("/healthz");

app.Run();
