import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AssuntoService } from '../../../core/services/assunto.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-assunto-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './assunto-form.html',
  styleUrl: './assunto-form.css'
})
export class AssuntoForm implements OnInit {
  assuntoForm: FormGroup;
  isEditMode = false;
  assuntoId: string | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private assuntoService: AssuntoService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService
  ) {
    this.assuntoForm = this.fb.group({
      descricao: ['', [Validators.required, Validators.maxLength(20)]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.assuntoId = id;
      this.loadAssunto(this.assuntoId);
    }
  }

  loadAssunto(id: string): void {
    this.assuntoService.getById(id).subscribe({
      next: (assunto) => {
        this.assuntoForm.patchValue({ descricao: assunto.descricao });
      },
      error: (err) => {
        this.error = 'Erro ao carregar assunto';
        this.toastService.error('Erro ao carregar assunto');
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.assuntoForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = null;

    const assuntoData = this.assuntoForm.value;

    if (this.isEditMode && this.assuntoId) {
      this.assuntoService.update(this.assuntoId, assuntoData).subscribe({
        next: () => {
          this.toastService.success('Assunto atualizado com sucesso!');
          this.router.navigate(['/assuntos']);
        },
        error: (err: any) => {
          this.error = 'Erro ao salvar assunto';
          this.toastService.error('Erro ao atualizar assunto');
          this.loading = false;
          console.error(err);
        }
      });
    } else {
      this.assuntoService.create(assuntoData).subscribe({
        next: () => {
          this.toastService.success('Assunto criado com sucesso!');
          this.router.navigate(['/assuntos']);
        },
        error: (err: any) => {
          this.error = 'Erro ao salvar assunto';
          this.toastService.error('Erro ao criar assunto');
          this.loading = false;
          console.error(err);
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/assuntos']);
  }
}
