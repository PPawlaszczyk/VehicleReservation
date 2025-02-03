import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';
import { environment } from '../../environments/environment';
import { CreateReservationDto } from '../../app/_modules/CreateReservationDto';
import { Vehicle } from '../../app/_modules/Vehicle';
import { VehicleType } from '../../app/_modules/enums';


@Component({
  selector: 'app-vehicle',
  templateUrl: './reservations-search.component.html',
  styleUrl: './reservations-search.component.css',
  standalone: true,
  imports: [FormsModule],
})

export class ReservationSearchComponent {
  vehicleTypes = Object.values(VehicleType).filter(value => typeof value === 'string');
  availableVehicles: Vehicle[] = [];
  baseUrl = environment.apiUrl;

  startDate: string = '2025-10-25';
  endDate: string = '2025-10-26';
  selectedVehicleType: VehicleType = VehicleType.Car;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getAvailableReservations();
  }
  getAvailableReservations() {
    const params = {
      startDate: this.startDate,
      endDate: this.endDate,
      type: VehicleType[this.selectedVehicleType]
    };

    this.http.get<Vehicle[]>( this.baseUrl + 'Vehicle/get-available-vehicles', { params })
      .pipe(
        map(vehicles => {
          console.log('API Response:', vehicles);
          if (vehicles) {
            this.availableVehicles = vehicles;
          }
          return vehicles;
        })
      )
      .subscribe({
        next: (vehicles) => console.log('Data received:', vehicles),
        error: (err) => console.error('Error fetching data:', err) 
      });
  }

  resetFilters() {
    this.startDate = '';
    this.endDate = '';
    this.selectedVehicleType = VehicleType.Car;
  }
  
  setReservation(vehicleId: string) {
    const reservation: CreateReservationDto = {
      startDate: this.startDate,
      vehicleId: vehicleId,
      endDate: this.endDate
    }

    return this.http.post<CreateReservationDto>( this.baseUrl + 'reservation/create-reservation-vehicle', reservation)
    .pipe(
      map(reservation => {
        console.log('API Response:', reservation);
        this.removeVehicle(vehicleId);
      })
    )
    .subscribe({
      next: (reservation) => console.log('Data received:', reservation),
      error: (err) => console.error('Error fetching data:', err) 
    })
  }

  removeVehicle(vehicleId: string): void {
    this.availableVehicles = this.availableVehicles.filter(vehicle => vehicle.vehicleId !== vehicleId);
  } 
}