import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.css'
})
export class AdminLoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  constructor(private router: Router) { }

  login():void{
    if(this.username==='admin' && this.password==='password'){
      this.router.navigate(['./admin-dashboard']);
    }
    else{
      this.errorMessage='Invalid username or password';
    }
  }
}

