import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './shared/auth.service';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(): Observable<boolean> {
    return this.authService.isLoggedIn().pipe(
      switchMap(isLoggedIn => {
        if (isLoggedIn) {
          const token = localStorage.getItem('token');
          if (!token) {
            this.router.navigate(['/login']);
            return of(false);
          }
          return this.authService.validateToken(token).pipe(
            map(isValid => {
              if (isValid) {
                return true;
              } else {
                this.handleInvalidToken();
                return false;
              }
            }),
            catchError(() => {
              this.handleInvalidToken();
              return of(false);
            })
          );
        } else {
          this.router.navigate(['/login']);
          return of(false);
        }
      })
    );
  }

  private handleInvalidToken(): void {
    localStorage.removeItem('token');
    this.authService.setLoggedIn(false);
    this.router.navigate(['/login']);
  }
}
