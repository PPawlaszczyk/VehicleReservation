﻿namespace VehicleReservationAPI.DTOs
{
    public record UserDto
    {
        public required string Username { get; init; }
        public required string Token { get; init; }
    }
}