export interface Assunto {
  codAs: string;
  descricao: string;
}

export interface AssuntoCreateDto {
  descricao: string;
}

export interface AssuntoUpdateDto {
  id: string;
  descricao: string;
}
