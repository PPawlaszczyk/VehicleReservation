import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';

enum VehicleType {
  Car = 'Car',
  Truck = 'Truck',
  Motorcycle = 'Motorcycle',
}

interface Vehicle {
  vehicleId: string;
  name: string;
  type: VehicleType;
  mark: string;
  seats: number;
  fuel: string;
  year: number;
  cost: number;
}

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

    this.http.get<Vehicle[]>('http://localhost:5000/api/Vehicle/get-available-vehicles', { params })
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

  bookVehicle(vehicleId: string) {
  }
  
}