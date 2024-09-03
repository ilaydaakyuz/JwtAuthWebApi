import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { tap, catchError, map, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5020/api';
  private loggedIn = new BehaviorSubject<boolean>(false);
  private tokenKey = 'token';

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.loggedIn.next(this.hasToken());
  }

  private hasToken(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return !!localStorage.getItem(this.tokenKey);
    }
    return false;
  }

  login(clientId: string, clientSecret: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/auth/login`, { clientId, clientSecret }).pipe(
      tap(response => {
        if (response && response.token) {
          this.setToken(response.token);
          this.loggedIn.next(true);
        }
      }),
      catchError(this.handleError)
    );
  }
  register(clientId: string, clientSecret: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/auth/register`, { clientId, clientSecret }).pipe(
      catchError(this.handleError)
    );
  }

  handleLogin(clientId: string, clientSecret: string) {
    this.login(clientId, clientSecret).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        console.error('Login failed', error);
        this.loggedIn.next(false);
      }
    });
  }

  logout(): void {
    this.removeToken();
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  validateToken(token: string): Observable<boolean> {
    return this.http.get<{ message: string }>(`${this.apiUrl}/auth/validate`, {
      headers: { Authorization: `Bearer ${token}` }
    }).pipe(
      map(response => {
        // Yanıtın message özelliğini kontrol et
        if (response && response.message === 'Token validation successful.') {
          return true;
        } else {
          return false;
        }
      }),
      catchError(error => {
        console.error('Token validation error:',error);
        return of(false);
      })
    );
  }
  

  private setToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.tokenKey, token);
    }
  }

  private removeToken(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.tokenKey);
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.tokenKey);
    }
    return null;
  }

  private handleError(error: any) {
    console.error('An error occurred:', error);
    return of(null);
  }


  getUserInfo(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/auth/GetUserInfoWithRedis?userId=${userId}`).pipe(
      catchError(this.handleError)
    );
  }

  sendMessage(message: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/auth/SendMessage`, { message }).pipe(
      catchError(this.handleError)
    );
  }

  setLoggedIn(value: boolean): void {
    this.loggedIn.next(value);
    if (!value) {
      localStorage.removeItem('token');
    }
  }

}