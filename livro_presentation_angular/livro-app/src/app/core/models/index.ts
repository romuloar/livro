// Export livro.model first (contains Autor, Assunto, TipoCompra interfaces)
export * from './livro.model';

// Explicitly export only non-conflicting types
export type { Autor, AutorCreateDto, AutorUpdateDto } from './autor.model';
export type { Assunto, AssuntoCreateDto, AssuntoUpdateDto } from './assunto.model';
export type { TipoCompra } from './tipo-compra.model';
export * from './relatorio.model';
export * from './result.model';
