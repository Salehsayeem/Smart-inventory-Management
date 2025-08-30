import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ApiFeedbackInterceptor } from './api/api-feedback.interceptor';

import { NgxUiLoaderModule, NgxUiLoaderConfig, NgxUiLoaderHttpModule } from 'ngx-ui-loader';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { FeaturesModule } from './features/features-module';
import { SideNav } from "./components/side-nav/side-nav";
import { MainPage } from "./features/main-page/main-page";
const PRIMARY_COLOR = "#006d33";

const ngxUiLoaderConfig: NgxUiLoaderConfig = {
  bgsColor: PRIMARY_COLOR,
  bgsOpacity: 0.5,
  bgsPosition: "bottom-right",
  bgsSize: 90,
  bgsType: "square-jelly-box",
  blur: 5,
  delay: 0,
  fastFadeOut: true,
  fgsColor: PRIMARY_COLOR,
  fgsPosition: "center-center",
  fgsSize: 60,
  fgsType: "square-jelly-box",
  gap: 24,
  logoPosition: "center-center",
  logoSize: 120,
  logoUrl: "",
  masterLoaderId: "master",
  overlayBorderRadius: "0",
  overlayColor: "rgba(40, 40, 40, 0.8)",
  pbColor: PRIMARY_COLOR,
  pbDirection: "ltr",
  pbThickness: 4,
  hasProgressBar: true,
  text: "Please wait...",
  textColor: "#FFFFFF",
  textPosition: "center-center",
  maxTime: -1,
  minTime: 300
};

@NgModule({
  declarations: [
    App
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
    NgxUiLoaderHttpModule.forRoot({ showForeground: true }),
    FeaturesModule,
    SideNav,
    MainPage
],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: ApiFeedbackInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }
