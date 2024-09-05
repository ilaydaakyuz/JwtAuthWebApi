import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = "http://localhost:5020/api/User";

  constructor(private http: HttpClient) { }
  createUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Create`, user).pipe(
      catchError(this.handleError)
    );
  }
  updateUser(userId: string, user: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Update?userId=${userId}`, user).pipe(
      catchError(this.handleError)
    );
  }
  readUser(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Read?userId=${userId}`).pipe(
      catchError(this.handleError)
    );
  }
  deleteUser(userId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Delete?userId=${userId}`).pipe(
      catchError(this.handleError)
    );
  }
  getAllUsers(): Observable<any>{
    return this.http.get<any[]>(`${this.apiUrl}/GetAllUsers`)
  }
  private handleError(error: any): Observable<never> {
    console.error('An error occurred: ', error);
    return throwError(error);
  }
}
