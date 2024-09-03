import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../shared/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [CommonModule, FormsModule, RouterModule],
  standalone: true
})
export class RegisterComponent {
  clientId: string = '';
  clientSecret: string = '';
 
  errorMessage: string = ''; // <---- Bu satırı ekle

  constructor(private authService: AuthService, private router: Router) { }

  register() {

    // Registration logic here
    this.authService.register(this.clientId, this.clientSecret).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.errorMessage = 'Registration failed';
        console.error('Registration failed', error);
      }
    });
  }
}
