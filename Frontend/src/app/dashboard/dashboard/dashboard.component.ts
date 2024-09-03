import { Component } from '@angular/core';
import { AuthService } from '../../shared/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HttpClientModule], 
  providers: [AuthService], 
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'] 
})
export class DashboardComponent {
  constructor(private authService: AuthService, private router: Router) { }
}
