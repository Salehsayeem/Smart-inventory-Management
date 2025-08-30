import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class SnackbarService {
  private snackBar = inject(MatSnackBar);

  show(message: string, type: 'success' | 'error' = 'success') {
    const config: MatSnackBarConfig = {
      duration: 3500,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      panelClass: type === 'success' ? ['snackbar-success'] : ['snackbar-error'],
      // Custom animation classes can be added here if needed
    };
    this.snackBar.open(message, 'Close', config);
  }
}
