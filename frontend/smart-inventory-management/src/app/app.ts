import { Component, HostListener, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: false,
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  isLeftSidebarCollapsed = false;
  screenWidth = window.innerWidth;
  @HostListener('window:resize')
  onResize() {
    this.screenWidth = window.innerWidth;
    if (this.screenWidth < 768) {
      this.isLeftSidebarCollapsed = true;
    }
  }

  ngOnInit(): void {
    this.isLeftSidebarCollapsed = this.screenWidth < 768;
  }

  changeIsLeftSidebarCollapsed(isLeftSidebarCollapsed: boolean): void {
    this.isLeftSidebarCollapsed = isLeftSidebarCollapsed;
  }

}
