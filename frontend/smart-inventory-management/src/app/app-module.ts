import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ApiFeedbackInterceptor } from './api/api-feedback.interceptor';

 import { NgxUiLoaderModule, NgxUiLoaderConfig, NgxUiLoaderHttpModule } from 'ngx-ui-loader';

const ngxUiLoaderConfig: NgxUiLoaderConfig = {
  bgsColor: "#1b00ff",
  bgsOpacity: 0.5,
  bgsPosition: "bottom-right",
  bgsSize: 90,
  bgsType: "square-jelly-box",
  blur: 5,
  delay: 0,
  fastFadeOut: true,
  fgsColor: "#1b00ff",
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
  pbColor: "blue",
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
    MatSlideToggleModule,
    MatSnackBarModule,
    NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
    NgxUiLoaderHttpModule.forRoot({ showForeground: true }) // <-- Automatically shows loader on all HTTP calls
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(),
    { provide: HTTP_INTERCEPTORS, useClass: ApiFeedbackInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }
