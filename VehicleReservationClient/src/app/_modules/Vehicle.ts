import { VehicleType } from "./enums";

export interface Vehicle {
  vehicleId: string;
  name: string;
  type: VehicleType;
  mark: string;
  seats: number;
  fuel: string;
  year: number;
  cost: number;
}

