import { inject, Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState }  from '@microsoft/signalr';
import { User } from '../_modules/user';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  hubUrl = environment.hubsUrl;
  private hubConnection?: HubConnection;
  private toastr = inject(ToastrService);

  createHubConnection(user: User)
  {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'reservation', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()
      this.hubConnection.start().catch(error => console.log(error));

      this.hubConnection.on('ReceiveNotification', message => {
          this.toastr.warning(message)
      });
  }

  stopHubConnection()
  {
    if(this.hubConnection?.state == HubConnectionState.Connected)
      {
      this.hubConnection.stop().catch(error => console.log(error))
    }
  }
}