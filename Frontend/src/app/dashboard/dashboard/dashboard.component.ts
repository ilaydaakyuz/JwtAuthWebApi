import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuComponent } from '../menu/menu.component';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule, RouterModule,MenuComponent],
  providers: [AuthService],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  username: string = "";
  userInfo: any;
  showMessageForm:boolean=false;
  constructor(private authService: AuthService, private router: Router) { }
  ngOnInit(): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.authService.getUserInfo(userId).subscribe(
        data => {
          console.log(data);
          this.username = data.UserName;
          this.userInfo = data;
        },
        error => {
          console.error('Kullanıcı bilgisi alınırken hata oluştu', error);
        }
      );
    }
  }
  logout(): Observable<void> {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    return of();
  }

  navigateToProfile(): void {
    console.log('Navigating to profile with data:', this.userInfo);
    this.router.navigate(['/profile'], { state: { data: this.userInfo } });
  }

  openMessageForm(): void {
    this.showMessageForm = true;
    this.router.navigate(['/message']);
  }
}
