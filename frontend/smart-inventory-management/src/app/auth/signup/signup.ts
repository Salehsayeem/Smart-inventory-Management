import { Component } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-signup',
  standalone: false,
  templateUrl: './signup.html',
  styleUrl: './signup.scss'
})
export class Signup {
  signupForm: ReturnType<FormBuilder['group']>;

  loading = false;
  error: string | null = null;

  constructor(private fb: FormBuilder) {
    this.signupForm = this.fb.group({
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.signupForm.invalid) return;
    this.loading = true;
    // Call your AuthService here
    // .subscribe(
    //   success => { ... },
    //   err => { this.error = err.message; this.loading = false; }
    // );
  }
}
