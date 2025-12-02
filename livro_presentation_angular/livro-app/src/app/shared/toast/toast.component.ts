import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, Toast } from '../../core/services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="toast" 
         [class]="'toast toast-' + toast.type"
         [@slideIn]>
      <div class="toast-icon">
        <span *ngIf="toast.type === 'success'">✓</span>
        <span *ngIf="toast.type === 'error'">✕</span>
        <span *ngIf="toast.type === 'info'">ℹ</span>
        <span *ngIf="toast.type === 'warning'">⚠</span>
      </div>
      <div class="toast-message">{{ toast.message }}</div>
      <button class="toast-close" (click)="close()">×</button>
    </div>
  `,
  styles: [`
    .toast {
      position: fixed;
      top: 20px;
      right: 20px;
      min-width: 300px;
      max-width: 500px;
      padding: 16px 20px;
      border-radius: 8px;
      display: flex;
      align-items: center;
      gap: 12px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      z-index: 9999;
      animation: slideIn 0.3s ease-out;
      font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    }

    @keyframes slideIn {
      from {
        transform: translateX(400px);
        opacity: 0;
      }
      to {
        transform: translateX(0);
        opacity: 1;
      }
    }

    .toast-success {
      background-color: #10b981;
      color: white;
    }

    .toast-error {
      background-color: #ef4444;
      color: white;
    }

    .toast-info {
      background-color: #3b82f6;
      color: white;
    }

    .toast-warning {
      background-color: #f59e0b;
      color: white;
    }

    .toast-icon {
      font-size: 20px;
      font-weight: bold;
      flex-shrink: 0;
    }

    .toast-message {
      flex: 1;
      font-size: 14px;
      line-height: 1.5;
    }

    .toast-close {
      background: none;
      border: none;
      color: white;
      font-size: 24px;
      cursor: pointer;
      padding: 0;
      width: 24px;
      height: 24px;
      display: flex;
      align-items: center;
      justify-content: center;
      opacity: 0.8;
      flex-shrink: 0;
    }

    .toast-close:hover {
      opacity: 1;
    }
  `]
})
export class ToastComponent implements OnInit {
  toast: Toast | null = null;

  constructor(private toastService: ToastService) {}

  ngOnInit() {
    this.toastService.toast$.subscribe(toast => {
      this.toast = toast;
    });
  }

  close() {
    this.toastService.hide();
  }
}
