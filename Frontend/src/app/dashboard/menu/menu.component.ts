import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '../../shared/auth.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  imports:[CommonModule, FormsModule, RouterModule]
})
export class MenuComponent {
  username: string = "";
  userInfo: any;
  showMessageForm:boolean=false;
  constructor(private authService:AuthService,  private router: Router) { }

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
