import { Component } from '@angular/core';
import { AuthService } from '../../auth/auth-service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard {
  constructor(private authService: AuthService) {
    console.log('Dashboard component initialized');
    const data = this.authService.decodeToken();
    console.log('Decoded token data:', data);
  }

}
