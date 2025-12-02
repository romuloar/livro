import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './shared/navbar/navbar';
import { ToastComponent } from './shared/toast/toast.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, ToastComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'Sistema de Livros';
}

