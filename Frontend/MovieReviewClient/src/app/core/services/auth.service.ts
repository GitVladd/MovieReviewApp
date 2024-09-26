import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/users`; 

  constructor(private http: HttpClient) {}


  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }


  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap((response: any) => {
        localStorage.setItem('jwt_token', response.token);
      })
    );
  }


  isAuthenticated(): boolean {
    return !!localStorage.getItem('jwt_token');
  }


  logout(): void {
    localStorage.removeItem('jwt_token');
  }
}
