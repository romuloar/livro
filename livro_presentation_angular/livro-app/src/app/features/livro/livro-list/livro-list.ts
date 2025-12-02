import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LivroService } from '../../../core/services/livro.service';
import { ToastService } from '../../../core/services/toast.service';
import { Livro } from '../../../core/models';

@Component({
  selector: 'app-livro-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './livro-list.html',
  styleUrl: './livro-list.css'
})
export class LivroList implements OnInit {
  livros = signal<Livro[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private livroService: LivroService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadLivros();
  }

  loadLivros(): void {
    console.log('loadLivros iniciado');
    this.loading.set(true);
    this.error.set(null);
    this.livroService.getAll().subscribe({
      next: (data) => {
        console.log('Dados recebidos:', data);
        this.livros.set(Array.isArray(data) ? data : []);
        this.loading.set(false);
        console.log('Loading definido como false');
      },
      error: (err) => {
        console.error('Erro ao carregar:', err);
        this.error.set('Erro ao carregar livros');
        this.toastService.error('Erro ao carregar livros');
        this.loading.set(false);
      }
    });
  }

  deleteLivro(id: string): void {
    if (confirm('Deseja realmente excluir este livro?')) {
      this.livroService.delete(id).subscribe({
        next: () => {
          this.toastService.success('Livro excluído com sucesso!');
          this.loadLivros();
        }, 
        error: (err) => {
          this.error.set('Erro ao excluir livro');
          this.toastService.error('Erro ao excluir livro');
          console.error(err);
        }
      });
    }
  }
}

