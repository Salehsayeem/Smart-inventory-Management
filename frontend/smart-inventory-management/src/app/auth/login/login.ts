import { Component } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { ApiService } from '../../api/api-service';
import { Router } from '@angular/router';
import { AuthService } from '../auth-service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  loginForm: ReturnType<FormBuilder['group']>;

  loading = false;
  error: string | null = null;

  constructor(private fb: FormBuilder, private api: ApiService, private router: Router, private auth: AuthService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

  }

  onSubmit() {
    if (this.loginForm.invalid) return;
    this.loading = true;
    this.api.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.statusCode >= 200 && res.statusCode < 300) {
          this.auth.storeToken(res.data);
          this.router.navigate(['/dashboard']);
        } else {
          console.log('Login failed:', res.message);
        }
      },
      error: (err) => {
        this.loading = false;
        console.error('Login error:', err);
      }
    });
  }
}
