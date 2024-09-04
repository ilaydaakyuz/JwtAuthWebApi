import { Component } from '@angular/core';
import { AuthService } from '../../shared/auth.service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  clientId: string = '';
  clientSecret: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  login(): void {
    this.authService.login(this.clientId, this.clientSecret).subscribe({
      next: (response) => {
        if (response && response.token) {
          // Token'ı null veya undefined kontrolü
          if (response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('userId', response.userId); 
            console.log('Token set:', localStorage.getItem('token'));
            this.router.navigateByUrl('/dashboard');
            console.log('Navigation to dashboard successful');
          } else {
            this.errorMessage = 'Login failed';
          }
        } else {
          this.errorMessage = 'Login failed';
        }
      },
      error: (error) => {
        console.error('Login error', error);
        this.errorMessage = 'An error occurred during login';
      }
    });
  }
}