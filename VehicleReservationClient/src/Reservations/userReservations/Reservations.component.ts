import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './Reservations.component.html',
  styleUrl: './Reservations.component.css'
})
export class ReservationsComponent {
  baseUrl = environment.apiUrl;
  validationErrors: string[] = [];
}
