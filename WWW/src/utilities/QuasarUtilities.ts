import {
  //   Dialog,
  //   Loading,
  //   Notify,
  //   QDialogOptions,
  //   QSelect,
  QTableProps,
} from 'quasar';

import { SearchResponse } from './ApiUtilities';

export type QTablePagination = Exclude<QTableProps['pagination'], undefined>;

export type QTableRequestEvent = Parameters<
  Exclude<QTableProps['onRequest'], undefined>
>[0];

export default {
  handleSearchResponse<T>(
    request: SearchResponse<T>,
    pagination: QTablePagination,
    sourcePagination?: QTablePagination
  ): T[] {
    pagination.rowsNumber = request.totalRows;
    if (sourcePagination) {
      pagination.page = sourcePagination.page;
      pagination.descending = sourcePagination.descending;
      pagination.rowsPerPage = sourcePagination.rowsPerPage;
      pagination.sortBy = sourcePagination.sortBy;
    }

    return request.rows;
  },
};
