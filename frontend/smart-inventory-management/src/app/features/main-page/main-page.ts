
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Component, input, computed, Input } from '@angular/core';

@Component({
  selector: 'app-main-page',
  standalone: true,
  templateUrl: './main-page.html',
  imports: [CommonModule, RouterModule],
  styleUrl: './main-page.scss',
})
export class MainPage {
  @Input() isLeftSidebarCollapsed!: boolean;
  @Input() screenWidth!: number;

  get sizeClass(): string {
    if (this.isLeftSidebarCollapsed) {
      return '';
    }
    return this.screenWidth > 768 ? 'body-trimmed' : 'body-md-screen';
  }
}
