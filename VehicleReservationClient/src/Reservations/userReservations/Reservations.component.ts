import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../environments/environment';
import { map, Observable } from 'rxjs';
import { UserReservationDto } from '../../app/_modules/UserReservationDto';
import { CreateReservationDto } from '../../app/_modules/CreateReservationDto';
import { ReturnReservedVehicleCommand } from '../../app/_modules/ReturnReservedVehicleCommand';
import { VehicleType } from '../../app/_modules/enums';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './Reservations.component.html',
  styleUrl: './Reservations.component.css'
})

export class ReservationsComponent {
  baseUrl = environment.apiUrl;
  reservations: UserReservationDto[] = [];
VehicleType: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getAllUserReservations();
  }
  
  getAllUserReservations() {
      this.http.get<UserReservationDto[]>( `${this.baseUrl}Reservation/my-reservations`)
        .pipe(
          map(reservations => {
            console.log('API Response:', reservations);
            if (reservations) {
              this.reservations = reservations;
            }
            return reservations;
          })
        )
        .subscribe({
          next: (reservations) => console.log('Data received:', reservations),
          error: (err) => console.error('Error fetching data:', err) 
        });
    }
  
  returnReservation(reservationId: string)
  {
    const reservation: ReturnReservedVehicleCommand = {
      reservationId: reservationId,
    }
        return this.http.post( this.baseUrl + 'reservation/return',reservation)
        .pipe(
          map(reservation => {
            console.log('API Response:', reservation);
            this.removeReservation(reservationId);
            return;
          })
        )
        .subscribe({
          next: (reservations) => console.log('Data received:', reservations),
          error: (err) => console.error('Error fetching data:', err) 
        });
  }

  removeReservation(reservationId: string): void {
    this.reservations = this.reservations.filter(reservation => reservation.id !== reservationId);
  } 
}