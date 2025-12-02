import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AutorService } from '../../../core/services/autor.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-autor-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './autor-form.html',
  styleUrl: './autor-form.css'
})
export class AutorForm implements OnInit {
  autorForm: FormGroup;
  isEditMode = false;
  autorId: string | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private autorService: AutorService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService
  ) {
    this.autorForm = this.fb.group({
      nome: ['', [Validators.required, Validators.maxLength(40)]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.autorId = id;
      this.loadAutor(this.autorId);
    }
  }

  loadAutor(id: string): void {
    this.autorService.getById(id).subscribe({
      next: (autor) => {
        this.autorForm.patchValue({ nome: autor.nome });
      },
      error: (err) => {
        this.error = 'Erro ao carregar autor';
        this.toastService.error('Erro ao carregar autor');
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.autorForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = null;

    const autorData = this.autorForm.value;

    if (this.isEditMode && this.autorId) {
      this.autorService.update(this.autorId, autorData).subscribe({
        next: () => {
          this.toastService.success('Autor atualizado com sucesso!');
          this.router.navigate(['/autores']);
        },
        error: (err: any) => {
          this.error = 'Erro ao salvar autor';
          this.toastService.error('Erro ao atualizar autor');
          this.loading = false;
          console.error(err);
        }
      });
    } else {
      this.autorService.create(autorData).subscribe({
        next: () => {
          this.toastService.success('Autor criado com sucesso!');
          this.router.navigate(['/autores']);
        },
        error: (err: any) => {
          this.error = 'Erro ao salvar autor';
          this.toastService.error('Erro ao criar autor');
          this.loading = false;
          console.error(err);
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/autores']);
  }
}

