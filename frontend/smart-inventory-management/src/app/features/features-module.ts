import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Dashboard } from './dashboard/dashboard';
import { SideNav } from '../components/side-nav/side-nav';
import { RouterModule } from '@angular/router';
import { MainPage } from './main-page/main-page';
import { Profile } from './profile/profile';



@NgModule({
  declarations: [
    Dashboard,
    Profile,
  ],
  imports: [
    CommonModule,
    SideNav,
    RouterModule,
    MainPage
  ]
})
export class FeaturesModule { }
