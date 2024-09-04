import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from '../shared/auth.service';


@Component({
  selector: 'app-message',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent {
  message: string = '';
  responseMessage: string = '';
  isSuccess: boolean = true;

  constructor(private authService: AuthService) { }

  sendMessage(): void {
    this.authService.sendMessage(this.message).subscribe(
      response => {
        console.log('Mesaj başarıyla gönderildi:', response);
        this.responseMessage = 'Mesaj başarıyla gönderildi!';
        this.isSuccess = true;
        this.message = '';
      },
      error => {
        this.responseMessage = 'Mesaj gönderilirken bir hata oluştu.';
        this.isSuccess = false;
        console.error('Mesaj gönderilirken bir hata oluştu:', error);
      }
    );
  }
}
