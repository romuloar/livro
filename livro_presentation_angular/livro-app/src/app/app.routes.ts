import { Routes } from '@angular/router';
import { AutorList } from './features/autor/autor-list/autor-list';
import { AutorForm } from './features/autor/autor-form/autor-form';
import { AssuntoList } from './features/assunto/assunto-list/assunto-list';
import { AssuntoForm } from './features/assunto/assunto-form/assunto-form';
import { LivroList } from './features/livro/livro-list/livro-list';
import { LivroForm } from './features/livro/livro-form/livro-form';
import { RelatorioView } from './features/relatorio/relatorio-view/relatorio-view';

export const routes: Routes = [
  { path: '', redirectTo: '/livros', pathMatch: 'full' },
  { path: 'livros', component: LivroList },
  { path: 'livros/novo', component: LivroForm },
  { path: 'livros/editar/:id', component: LivroForm },
  { path: 'autores', component: AutorList },
  { path: 'autores/novo', component: AutorForm },
  { path: 'autores/editar/:id', component: AutorForm },
  { path: 'assuntos', component: AssuntoList },
  { path: 'assuntos/novo', component: AssuntoForm },
  { path: 'assuntos/editar/:id', component: AssuntoForm },
  { path: 'relatorio', component: RelatorioView }
];

