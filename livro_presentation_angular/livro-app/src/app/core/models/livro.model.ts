export interface Livro {
  codl: string;
  titulo: string;
  editora: string;
  edicao: number;
  anoPublicacao: string;
  listAutor?: Autor[];
  listAssunto?: Assunto[];
  listLivroValor?: LivroValor[];
}

export interface Autor {
  codAu: string;
  nome: string;
}

export interface Assunto {
  codAs: string;
  descricao: string;
}

export interface LivroValor {
  codlv?: string;
  livro_Codl?: string;
  tipoCompraId: string;
  tipoCompraDescricao?: string;
  valor: number;
}

export interface TipoCompra {
  codTc: string;
  descricao: string;
}

export interface LivroCreateDto {
  titulo: string;
  editora: string;
  edicao: number;
  anoPublicacao: string;
  autoresIds: string[];
  assuntosIds: string[];
  valores: LivroValorCreateDto[];
}

export interface LivroUpdateDto {
  id: string;
  titulo: string;
  editora: string;
  edicao: number;
  anoPublicacao: string;
  autoresIds: string[];
  assuntosIds: string[];
  valores: LivroValorUpdateDto[];
}

export interface LivroValorCreateDto {
  tipoCompraId: string;
  valor: number;
}

export interface LivroValorUpdateDto {
  tipoCompraId: string;
  valor: number;
}
