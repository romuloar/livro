export interface Autor {
  codAu: string;
  nome: string;
}

export interface AutorCreateDto {
  nome: string;
}

export interface AutorUpdateDto {
  id: string;
  nome: string;
}
