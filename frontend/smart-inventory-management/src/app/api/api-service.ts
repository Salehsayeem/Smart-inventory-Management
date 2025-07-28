import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest } from './schema/request/auth.request';
import { ApiResponse } from './schema/response/api-common.response';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  login(payload: LoginRequest): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      this.baseUrl + 'Auth/login',
      payload
    );
  }
}
