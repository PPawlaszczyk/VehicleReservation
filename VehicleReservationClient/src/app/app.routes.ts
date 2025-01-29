import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { ReservationsComponent } from '../Reservations/userReservations/Reservations.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { ReservationSearchComponent } from '../Reservations/searchReservation/reservations-search.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: '', runGuardsAndResolvers:'always', 
        canActivate:[authGuard], 
        children: [
            {path:'reservations', component: ReservationsComponent},
            {path:'reservations-search', component: ReservationSearchComponent},

    ]},
    {path:'server-error', component: ServerErrorComponent},
    {path: '**', component: HomeComponent, pathMatch: 'full'},
];
