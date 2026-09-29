import { AxiosResponse } from 'axios';
import _ from 'lodash';

export interface SearchRequest {
  page?: number;
  pagesize?: number;
  ordering?: string;
}

export interface SearchResponse<T> {
  rows: T[];
  totalRows: number;
}
