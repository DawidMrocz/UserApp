import _ from 'lodash';
import FilterUtilities, { Filter } from 'src/utilities/FilterUtilities';
import { QTablePagination } from 'src/utilities/QuasarUtilities';
import { ref } from 'vue';

export default function (filter: Filter, value: QTablePagination) {
  const pagination = ref<QTablePagination>(value);

  return {
    pagination,
    storePagination: (): void => {
      FilterUtilities.storePagination(filter, pagination.value);
    },
    loadPagination: (): void => {
      const loaded = FilterUtilities.loadPagination(filter);
      if (!_.isEmpty(loaded)) {
        pagination.value = loaded;
      }
    },
  };
}
