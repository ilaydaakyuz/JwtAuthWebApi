import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private hubConnection!: signalR.HubConnection;
  public activeUsers: string[] = [];

  constructor() { }
  public startConnection(token: string, username: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5020/chathub?username=${encodeURIComponent(username)}`, { accessTokenFactory: () => token })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR bağlantısı başarılı.'))
      .catch(err => console.log('SignalR bağlantı hatası', err));
    this.addUserListListener();
  }

  public addMessageListener(): void {
    this.hubConnection.on('ReceiveMessage', (message) => {
      console.log('Gelen mesaj:', message);
    });
  }

  public sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message)
      .catch(err => console.error('Mesaj gönderilemedi: ', err));
  }

  public addUserListListener(): void {
    this.hubConnection.on('UpdateUserList', (users: string[]) => {
      this.activeUsers = users;
      console.log('Aktif kullanıcılar:', this.activeUsers);
    });
  }
}
