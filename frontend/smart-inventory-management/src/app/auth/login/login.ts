import { Component } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { ApiService } from '../../api/api-service';
import { SnackbarService } from '../../components/snackbar/snackbar.service';

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

  constructor(private fb: FormBuilder, private api: ApiService,  private snackbar: SnackbarService) {
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
        this.snackbar.show(res.message || 'Login successful', 'success');
        // ...handle success
      } else {
        this.snackbar.show(res.message || 'Login failed', 'error');
      }
    },
    error: (err) => {
      this.loading = false;
      this.snackbar.show(err.error?.message || 'Login failed', 'error');
    }
  });
}
}
