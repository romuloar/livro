export interface RelatorioLivro {
  autorNome: string;
  livroTitulo: string;
  editora: string;
  edicao: number;
  anoPublicacao: string;
  assuntos: string[];
  valores: ValorRelatorio[];
}

export interface ValorRelatorio {
  tipoCompra: string;
  valor: number;
}
