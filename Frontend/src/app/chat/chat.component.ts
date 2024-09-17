import { Component, OnDestroy, OnInit } from '@angular/core';
import { ChatService } from '../shared/chat.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MenuComponent } from '../dashboard/menu/menu.component';
import { AuthService } from '../shared/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, RouterModule, MenuComponent],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit,OnDestroy {
  message: string = '';
  token: string = '';
  userId: string = '';
  username: string = '';
  selectedUser: string = '';
  messages: { user: string, message: string }[] = [];
  activeUsers: string[] = [];
  notification: string | null = null;
  private messageSubscription: Subscription | null = null;
  private activeUsersSubscription: Subscription | null = null;

  constructor(private chatService: ChatService, private authService: AuthService) {
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
      this.userId = localStorage.getItem('userId') || '';
      console.log('Token:', this.token);
    } else {
      console.error('localStorage is not available');
    }
  }

  ngOnInit(): void {
    if (this.userId) {
      this.authService.getUserInfo(this.userId).subscribe(userInfo => {
        this.username = userInfo.UserName;
        this.chatService.startConnection(this.token, this.username);
        

        this.chatService.activeUsers$.subscribe(users => {
          this.activeUsers = users;
          this.chatService.addMessageListener((user, message) => {
            this.messages.push({ user, message });
            this.showNotification(`New message from ${user}: ${message}`);
          });
        });
        this.messageSubscription = this.chatService.messages$.subscribe(msgs => {
          this.messages = msgs;
        });
      });
    } else {
      console.error('User ID is not available');
    }
  }
  ngOnDestroy(): void {
    // Bileşen yok edilirken abonelikleri temizle
    this.messageSubscription?.unsubscribe();
    this.activeUsersSubscription?.unsubscribe();
  }

  selectUser(user: string): void {
    this.selectedUser = user;
  }

  sendPrivateMessage(): void {
    if (this.selectedUser && this.message.trim() !== '') {
      this.chatService.sendPrivateMessage(this.selectedUser, this.message);
      this.message = '';
    }
  }
  private showNotification(message: string): void {
    this.notification = message;
    setTimeout(() => this.notification = null, 5000);
  }
}
