import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs';
import { AuthService } from './auth.service'; // Import the AuthService

export interface LogEntry {
  service: string;
  level: string;
  message: string;
  timestamp: string;
}

export interface LogResponse {
  logs: LogEntry[];
  totalCount: number;
}

@Injectable({
  providedIn: 'root',
})
export class LogService {
  private apiUrl = 'https://localhost:7136/v1/logs/'; //API URL

  constructor(private http: HttpClient, private authService: AuthService) { }

  // Fetch logs from the API
  getLogs(filters: any, page: number = 1, pageSize: number = 5): Observable<LogResponse> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('page_size', pageSize.toString());

    if (filters.service) {
      params = params.set('service', filters.service);
    }
    if (filters.level) {
      params = params.set('level', filters.level);
    }
    if (filters.startTime) {
      params = params.set('startTime', filters.startTime);
    }
    if (filters.endTime) {
      params = params.set('endTime', filters.endTime);
    }

    // Add the Authorization header with the Bearer token
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.authService.getToken()}`);

    return this.http.get<{ logs: LogEntry[]; totalCount: number }>(this.apiUrl, { params, headers }).pipe(
      map((response) => ({
        logs: response.logs,
        totalCount: response.totalCount,
      }))
    );
  }
}
