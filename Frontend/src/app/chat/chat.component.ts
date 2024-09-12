import { Component, OnInit } from '@angular/core';
import { ChatService } from '../shared/chat.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MenuComponent } from '../dashboard/menu/menu.component';
import { UserService } from '../shared/user.service';
import { userInfo } from 'os';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, RouterModule, MenuComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit {
  message: string = '';
  token: string = '';
  userId: string='';
  username: string='';
  messages: { user: string, message: string }[] = [];

  constructor(private chatService: ChatService, private userService:UserService) {
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
      this.userId=localStorage.getItem('userId') || '';
      console.log('Token:', this.token);
    } else {
      console.error('localStorage is not available');
    }

  }

  ngOnInit(): void {
    if(this.userId){
      this.userService.readUser(this.userId).subscribe(userInfo=>{
        this.username=userInfo.username;
        this.chatService.startConnection(this.token,this.username);
        this.chatService.addMessageListener();
        this.chatService.addUserListListener();
      })
    }
    
  }
  sendMessage(): void {
    this.chatService.sendMessage(this.message);
    this.message = '';
  }
  get activeUsers(): string[] {
    return this.chatService.activeUsers;
  }

}
