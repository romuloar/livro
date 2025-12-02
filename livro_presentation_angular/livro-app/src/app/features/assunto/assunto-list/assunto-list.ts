import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AssuntoService } from '../../../core/services/assunto.service';
import { ToastService } from '../../../core/services/toast.service';
import { Assunto } from '../../../core/models';

@Component({
  selector: 'app-assunto-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './assunto-list.html',
  styleUrl: './assunto-list.css'
})
export class AssuntoList implements OnInit {
  assuntos = signal<Assunto[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private assuntoService: AssuntoService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadAssuntos();
  }

  loadAssuntos(): void {
    console.log('loadAssuntos iniciado');
    this.loading.set(true);
    this.error.set(null);
    this.assuntoService.getAll().subscribe({
      next: (data) => {
        console.log('Dados recebidos:', data);
        this.assuntos.set(Array.isArray(data) ? data : []);
        this.loading.set(false);
        console.log('Loading definido como false');
      },
      error: (err) => {
        console.error('Erro ao carregar:', err);
        this.error.set('Erro ao carregar assuntos');
        this.toastService.error('Erro ao carregar assuntos');
        this.loading.set(false);
      }
    });
  }

  deleteAssunto(id: string): void {
    if (confirm('Deseja realmente excluir este assunto?')) {
      this.assuntoService.delete(id).subscribe({
        next: () => {
          this.toastService.success('Assunto excluído com sucesso!');
          this.loadAssuntos();
        },
        error: (err) => {
          this.error.set('Erro ao excluir assunto');
          this.toastService.error('Erro ao excluir assunto');
          console.error(err);
        }
      });
    }
  }
}
