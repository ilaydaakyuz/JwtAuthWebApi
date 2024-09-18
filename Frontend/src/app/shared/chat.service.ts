import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private hubConnection!: signalR.HubConnection;

  private messagesSubject: BehaviorSubject<{ user: string, message: string }[]> = new BehaviorSubject<{ user: string, message: string }[]>([]);
  public messages$: Observable<{ user: string, message: string }[]> = this.messagesSubject.asObservable();
  private activeUsersSubject: BehaviorSubject<string[]> = new BehaviorSubject<string[]>([]);
  public activeUsers$: Observable<string[]> = this.activeUsersSubject.asObservable();
  private userMessages: { [username: string]: { user: string, message: string }[] } = {};

  constructor() { }

  // SignalR bağlantısını başlatan fonksiyon
  public startConnection(token: string, username: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5020/chathub?username=${encodeURIComponent(username)}`, { accessTokenFactory: () => token })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR bağlantısı başarılı.');
        this.addUserListListener();
      })
      .catch(err => console.log('SignalR bağlantı hatası', err));
  }

  public addMessageListener(on: (user: string, message: string) => void): void {
    this.hubConnection.off('ReceiveMessage');

    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      if (!this.userMessages[user]) {
        this.userMessages[user] = [];
      }
      console.log(`Gelen mesaj: ${message} | Gönderen: ${user}`);

      this.userMessages[user].push({ user, message });

    });
  }


  // Özel mesaj gönderme fonksiyonu
  public sendPrivateMessage(receiver: string, message: string): void {
    this.hubConnection.invoke('SendPrivateMessage', receiver, message)
      .then(() => {
        if (!this.userMessages[receiver]) {
          this.userMessages[receiver] = [];
        }
        this.userMessages[receiver].push({ user: 'Me', message });
      })
      .catch(err => console.error('Özel mesaj gönderilemedi: ', err));
  }
  public getMessagesForUser(username: string): { user: string, message: string }[] {
    return this.userMessages[username] || [];
  }

  // Aktif kullanıcı listesini dinleyen fonksiyon
  public addUserListListener(): void {
    this.hubConnection.on('UpdateUserList', (users: string[]) => {
      console.log('Aktif kullanıcılar:', users);

      // BehaviorSubject üzerinden aktif kullanıcıları güncelleyin
      this.activeUsersSubject.next(users);
    });
  }
}
