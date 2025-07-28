import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Login } from './login/login';
import { Signup } from './signup/signup';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    Login,
    Signup
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule

  ]
})
export class AuthModule { }
