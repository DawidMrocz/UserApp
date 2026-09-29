import _ from 'lodash';
import SerializerUtilities, {
  SerializerDeclaration,
} from './SerializerUtilities';
import { QTablePagination } from 'src/utilities/QuasarUtilities';

export enum Filter {
  ORDER_PREPAID = 'order_prepaid',
  ORDER_PHI = 'order_phi',
  LEAD = 'lead',
  SALES_PERSON = 'sales_person',
  ACTIVITY = 'activity',
  RENEWAL_PREPAID = 'renewal_prepaid',
  RENEWAL_PHI = 'renewal_phi',
  CONSOLIDATION = 'consolidation',
  REPORT_CONVERSION = 'report_conversion',
  OFFER = 'offer',
}

export enum FilterPrefix {
  FILTER = 'filter',
  PAGINATION = 'pagination',
}

export const FilterExclusionGroups: Filter[][] = [
  [
    Filter.LEAD,
    Filter.ORDER_PHI,
    Filter.ORDER_PREPAID,
    Filter.RENEWAL_PHI,
    Filter.RENEWAL_PREPAID,
  ],
];

export default {
  generateKey(prefix: FilterPrefix, filter: Filter): string {
    return `nesp__${prefix}_${filter}`;
  },
  storeData(
    prefix: FilterPrefix,
    filter: Filter,
    data: Record<string, unknown>
  ): void {
    sessionStorage.setItem(
      this.generateKey(prefix, filter),
      JSON.stringify(data)
    );
  },
  loadData(prefix: FilterPrefix, filter: Filter): Record<string, unknown> {
    const filterText = sessionStorage.getItem(this.generateKey(prefix, filter));
    if (_.isNil(filterText)) {
      return {};
    }

    try {
      return JSON.parse(filterText);
    } catch (err) {
      return {};
    }
  },
  storeFilter(filter: Filter, data: Record<string, unknown>): void {
    this.storeData(FilterPrefix.FILTER, filter, data);
  },
  storeSerializedFilter(
    key: Filter,
    data: Record<string, unknown>,
    declaration: SerializerDeclaration
  ): void {
    this.storeFilter(
      key,
      SerializerUtilities.serializeObject(data, declaration)
    );
  },
  loadFilter(filter: Filter): Record<string, unknown> {
    return this.loadData(FilterPrefix.FILTER, filter);
  },
  loadSerializedFilter(
    key: Filter,
    declaration: SerializerDeclaration
  ): Record<string, unknown> {
    return SerializerUtilities.deserializeObject(
      this.loadFilter(key),
      declaration
    );
  },
  storePagination(filter: Filter, pagination: QTablePagination): void {
    this.storeData(FilterPrefix.PAGINATION, filter, pagination);
  },
  loadPagination(filter: Filter): QTablePagination {
    const data = this.loadData(FilterPrefix.PAGINATION, filter);
    if (!_.isNil(data.page)) {
      data.page = 1;
    }
    return data;
  },
  clearGroup(filter: Filter): void {
    const filters = _.chain(FilterExclusionGroups)
      .flatMap((group) =>
        group.includes(filter) ? group.filter((f) => f !== filter) : []
      )
      .uniq()
      .value();
    Object.values(filters).forEach((f) => this.clearFilter(f));
  },
  clearFilter(filter: Filter): void {
    Object.values(FilterPrefix).forEach((prefix) =>
      sessionStorage.removeItem(this.generateKey(prefix, filter))
    );
  },
  clearFilters(): void {
    Object.values(Filter).forEach((f) => this.clearFilter(f));
  },
};
