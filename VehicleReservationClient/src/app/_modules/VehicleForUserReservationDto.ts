import { VehicleType } from "./enums";

export interface VehicleForUserReservationDto {
  vehicleId: string;
  name: string;
  type: VehicleType;
  mark: string;
  seats: number;
  fuel: string;
  year: number;
  cost: number;
  registrationNumber: string;
}