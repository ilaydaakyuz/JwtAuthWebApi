import { Component, OnDestroy, OnInit } from '@angular/core';
import { ChatService } from '../shared/chat.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MenuComponent } from '../dashboard/menu/menu.component';
import { AuthService } from '../shared/auth.service';
import { Subscription } from 'rxjs';
import { TranslationService } from '../shared/translation.service';
import { response } from 'express';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, RouterModule, MenuComponent],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit,OnDestroy {
  message: string = '';
  translatedMessage:string='';
  selectedLanguage:string='tr';
  token: string = '';
  userId: string = '';
  username: string = '';
  selectedUser: string = '';
  messages: { user: string, message: string }[] = [];
  activeUsers: string[] = [];
  notification: string | null = null;
  languages = [
    { code: 'en', name: 'English' },
    { code: 'fr', name: 'French' },
    { code: 'es', name: 'Spanish' },
    { code: 'de', name: 'German' },
    { code: 'tr', name: 'Turkish' }
  ];
  private messageSubscription: Subscription | null = null;
  private activeUsersSubscription: Subscription | null = null;

  constructor(private chatService: ChatService, private authService: AuthService,private translationService:TranslationService) {
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
    this.messages = this.chatService.getMessagesForUser(user);
  }

  sendPrivateMessage(): void {
    if (this.selectedUser && this.message.trim() !== '') {
      if(this.selectedLanguage!='tr'){
      this.translationService.translateText(this.message, 'tr', this.selectedLanguage).subscribe(translatedMessage => {
        this.chatService.sendPrivateMessage(this.selectedUser, translatedMessage);
        this.message = '';
      });
    }
    else{
      this.chatService.sendPrivateMessage(this.selectedUser,this.message);
      this.message='';
    }
    }
  }
 
  private showNotification(message: string): void {
    this.notification = message;
    setTimeout(() => this.notification = null, 5000);
  }
}
