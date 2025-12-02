export interface ApiResult<T> {
  isSuccess: boolean;
  message: string;
  resultData: T;
}
