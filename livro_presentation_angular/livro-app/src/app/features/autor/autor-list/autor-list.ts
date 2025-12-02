import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AutorService } from '../../../core/services/autor.service';
import { ToastService } from '../../../core/services/toast.service';
import { Autor } from '../../../core/models';

@Component({
  selector: 'app-autor-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './autor-list.html',
  styleUrl: './autor-list.css'
})
export class AutorList implements OnInit {
  autores = signal<Autor[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private autorService: AutorService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadAutores();
  }

  loadAutores(): void {
    console.log('loadAutores iniciado');
    this.loading.set(true);
    this.error.set(null);
    this.autorService.getAll().subscribe({
      next: (data) => {
        console.log('Dados recebidos:', data);
        this.autores.set(Array.isArray(data) ? data : []);
        this.loading.set(false);
        console.log('Loading definido como false');
      },
      error: (err) => {
        console.error('Erro ao carregar:', err);
        this.error.set('Erro ao carregar autores');
        this.toastService.error('Erro ao carregar autores');
        this.loading.set(false);
      }
    });
  }

  deleteAutor(id: string): void {
    if (confirm('Deseja realmente excluir este autor?')) {
      this.autorService.delete(id).subscribe({
        next: () => {
          this.toastService.success('Autor excluído com sucesso!');
          this.loadAutores();
        },
        error: (err) => {
          this.error.set('Erro ao excluir autor');
          this.toastService.error('Erro ao excluir autor');
          console.error(err);
        }
      });
    }
  }
}

