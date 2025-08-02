import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AuthResponse, PermissionDto } from '../api/schema/response/auth-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'token';

  constructor(private cookieService: CookieService) {}

  storeToken(token: string) {
    this.cookieService.set(this.TOKEN_KEY, token, undefined, '/');
  }

  getToken(): string | null {
    return this.cookieService.get(this.TOKEN_KEY) || null;
  }

  decodeToken(): AuthResponse | null {
    const token = this.getToken();
    if (!token) return null;
    const payload = token.split('.')[1];
    if (!payload) return null;
    try {
      const decoded = JSON.parse(atob(payload));
      let permissions: PermissionDto | null = null;
      if (decoded.Permissions) {
        permissions = typeof decoded.Permissions === 'string'
          ? JSON.parse(decoded.Permissions)
          : decoded.Permissions;
      }
      return {
        sub: decoded.sub,
        email: decoded.email,
        name: decoded.name,
        shops: decoded.Shops,
        role: decoded.Role,
        permissions: permissions!,
        nbf: decoded.nbf,
        exp: decoded.exp,
        iat: decoded.iat,
        iss: decoded.iss,
        aud: decoded.aud
      };
    } catch {
      return null;
    }
  }

  clearToken() {
    this.cookieService.delete(this.TOKEN_KEY, '/');
  }
}
