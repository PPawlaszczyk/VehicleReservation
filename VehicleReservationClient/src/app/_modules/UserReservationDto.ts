import { VehicleForUserReservationDto } from "./VehicleForUserReservationDto";

export interface UserReservationDto {
  id: string;
  vehicle: VehicleForUserReservationDto;
  endDate: string;
  startDate: string;
}