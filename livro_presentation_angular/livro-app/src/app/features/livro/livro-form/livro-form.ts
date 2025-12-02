import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LivroService } from '../../../core/services/livro.service';
import { AutorService } from '../../../core/services/autor.service';
import { AssuntoService } from '../../../core/services/assunto.service';
import { TipoCompraService } from '../../../core/services/tipo-compra.service';
import { ToastService } from '../../../core/services/toast.service';
import { Autor, Assunto, TipoCompra } from '../../../core/models';

@Component({
  selector: 'app-livro-form',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './livro-form.html',
  styleUrl: './livro-form.css'
})
export class LivroForm implements OnInit {
  livroForm: FormGroup;
  isEditMode = false;
  livroId: string | null = null;
  loading = false;
  error: string | null = null;

  autores: Autor[] = [];
  assuntos: Assunto[] = [];
  tiposCompra: TipoCompra[] = [];
  
  // Campos para o formulário de adição de valores
  tipoCompraSelecionado: string | null = null;
  valorSelecionado: number | null = null;
  
  loadingTiposCompra = true;

  constructor(
    private fb: FormBuilder,
    private livroService: LivroService,
    private autorService: AutorService,
    private assuntoService: AssuntoService,
    private tipoCompraService: TipoCompraService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private toastService: ToastService
  ) {
    this.livroForm = this.fb.group({
      titulo: ['', [Validators.required, Validators.maxLength(40)]],
      editora: ['', [Validators.maxLength(40)]],
      edicao: [null, [Validators.required]],
      anoPublicacao: ['', [Validators.required, Validators.maxLength(4)]],
      autores: [[]],
      assuntos: [[]],
      valores: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.loadData();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.livroId = id;
      this.loadLivro(this.livroId);
    }
  }

  loadData(): void {
    this.autorService.getAll().subscribe(autores => this.autores = autores || []);
    this.assuntoService.getAll().subscribe(assuntos => this.assuntos = assuntos || []);
    this.loadingTiposCompra = true;
    console.log('Iniciando carregamento de tipos de compra...');
    this.tipoCompraService.getAll().subscribe({
      next: (tipos) => {
        console.log('Tipos recebidos da API:', tipos);
        this.tiposCompra = Array.isArray(tipos) ? tipos : [];
        this.loadingTiposCompra = false;
        console.log('Tipos de compra carregados:', this.tiposCompra);
        console.log('loadingTiposCompra:', this.loadingTiposCompra);
        this.cdr.detectChanges(); // Força detecção de mudanças
      },
      error: (err) => {
        console.error('Erro ao carregar tipos de compra:', err);
        this.tiposCompra = [];
        this.loadingTiposCompra = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadLivro(id: string): void {
    this.livroService.getById(id).subscribe({
      next: (livro) => {
        this.livroForm.patchValue({
          titulo: livro.titulo,
          editora: livro.editora,
          edicao: livro.edicao,
          anoPublicacao: livro.anoPublicacao,
          autores: livro.listAutor?.map(a => a.codAu) || [],
          assuntos: livro.listAssunto?.map(a => a.codAs) || []
        });

        if (livro.listLivroValor && livro.listLivroValor.length > 0) {
          livro.listLivroValor.forEach(v => {
            this.addValor(v.tipoCompraId, v.valor);
          });
        }
      },
      error: (err) => {
        this.error = 'Erro ao carregar livro';
        console.error(err);
      }
    });
  }

  get valores(): FormArray {
    return this.livroForm.get('valores') as FormArray;
  }

  get tiposCompraDisponiveis(): TipoCompra[] {
    const tiposSelecionados = this.valores.controls
      .map(control => control.get('tipoCompraId')?.value)
      .filter(value => value !== null && value !== undefined);
    return this.tiposCompra.filter(tipo => !tiposSelecionados.includes(tipo.codTc));
  }

  isFormValid(): boolean {
    return this.livroForm.valid && this.valores.length > 0;
  }

  adicionarValorNaTabela(): void {
    if (this.tipoCompraSelecionado && this.valorSelecionado !== null && this.valorSelecionado >= 0) {
      this.addValor(this.tipoCompraSelecionado, this.valorSelecionado);
      this.tipoCompraSelecionado = null;
      this.valorSelecionado = null;
    }
  }

  getTipoCompraDescricao(codTc: string): string {
    const tipo = this.tiposCompra.find(t => t.codTc === codTc);
    return tipo ? tipo.descricao : '';
  }

  addValor(tipoCompraId?: string, valor?: number): void {
    const valorForm = this.fb.group({
      tipoCompraId: [tipoCompraId || null, Validators.required],
      valor: [valor || null, [Validators.required, Validators.min(0)]]
    });
    this.valores.push(valorForm);
  }

  removeValor(index: number): void {
    this.valores.removeAt(index);
  }

  onAutorChange(autorId: string, event: any): void {
    const autores = this.livroForm.get('autores')?.value as string[];
    if (event.target.checked) {
      this.livroForm.patchValue({ autores: [...autores, autorId] });
    } else {
      this.livroForm.patchValue({ autores: autores.filter(id => id !== autorId) });
    }
  }

  onAssuntoChange(assuntoId: string, event: any): void {
    const assuntos = this.livroForm.get('assuntos')?.value as string[];
    if (event.target.checked) {
      this.livroForm.patchValue({ assuntos: [...assuntos, assuntoId] });
    } else {
      this.livroForm.patchValue({ assuntos: assuntos.filter(id => id !== assuntoId) });
    }
  }

  isAutorSelected(autorId: string): boolean {
    const autores = this.livroForm.get('autores')?.value as string[];
    return autores.includes(autorId);
  }

  isAssuntoSelected(assuntoId: string): boolean {
    const assuntos = this.livroForm.get('assuntos')?.value as string[];
    return assuntos.includes(assuntoId);
  }

  onSubmit(): void {
    if (this.livroForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = null;

    const formValue = this.livroForm.value;
    
    // Validar se há pelo menos um autor e um assunto
    if (!formValue.autores || formValue.autores.length === 0) {
      this.error = 'É necessário selecionar pelo menos um autor';
      this.toastService.warning('É necessário selecionar pelo menos um autor');
      this.loading = false;
      return;
    }
    
    if (!formValue.assuntos || formValue.assuntos.length === 0) {
      this.error = 'É necessário selecionar pelo menos um assunto';
      this.toastService.warning('É necessário selecionar pelo menos um assunto');
      this.loading = false;
      return;
    }
    
    // Mapear para o formato esperado pela API
    const livroData = {
      titulo: formValue.titulo,
      editora: formValue.editora,
      edicao: formValue.edicao,
      anoPublicacao: formValue.anoPublicacao,
      autoresIds: formValue.autores,
      assuntosIds: formValue.assuntos,
      valores: formValue.valores
    };

    console.log('Dados enviados para API:', livroData);

    if (this.isEditMode && this.livroId) {
      const updateData = {
        id: this.livroId,
        ...livroData
      };
      this.livroService.update(this.livroId, updateData).subscribe({
        next: () => {
          this.toastService.success('Livro atualizado com sucesso!');
          this.router.navigate(['/livros']);
        },
        error: (err: any) => {
          this.error = 'Erro ao salvar livro';
          this.toastService.error('Erro ao atualizar livro');
          this.loading = false;
          console.error(err);
        }
      });
    } else {
      this.livroService.create(livroData).subscribe({
        next: () => {
          this.toastService.success('Livro criado com sucesso!');
          this.router.navigate(['/livros']);
        },
        error: (err: any) => {
          this.error = 'Erro ao salvar livro';
          this.toastService.error('Erro ao criar livro');
          this.loading = false;
          console.error(err);
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/livros']);
  }
}

