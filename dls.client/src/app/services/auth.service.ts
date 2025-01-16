import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';  // Import Router to handle redirection

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7136/v1/auth/login'; // backend login endpoint
  private token: string | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  // Login method
  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(this.apiUrl, { username, password });
  }

  // Save token in local storage
  saveToken(token: string): void {
    localStorage.setItem('token', token);
    this.token = token;
  }

  // Get token from local storage
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Check if the user is logged in
  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  // Logout the user by removing the token
  logout(): void {
    localStorage.removeItem('token');
    this.token = null;
  }

  // Add token to the HTTP headers
  getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  // Validate if the token is valid (optional improvement)
  validateToken(): boolean {
    const token = this.getToken();
    return token !== null; 
  }

  // user is authenticated or redirect to login
  redirectIfNotAuthenticated(): void {
    if (!this.isAuthenticated()) {
      this.router.navigate(['/login']);  // Redirect to login page if not authenticated
    }
  }
}
