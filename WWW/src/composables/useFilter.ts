import _ from 'lodash';
import FilterUtilities, { Filter } from 'src/utilities/FilterUtilities';
import { SerializerDeclaration } from 'src/utilities/SerializerUtilities';

export default function <T extends Record<string, unknown>>(
  key: Filter,
  composableDeclaration?: SerializerDeclaration
) {
  FilterUtilities.clearGroup(key);
  return {
    storeFilter: (data: T, declaration?: SerializerDeclaration): void => {
      const currentDeclaration = composableDeclaration ?? declaration;
      if (_.isNil(currentDeclaration)) {
        FilterUtilities.storeFilter(key, data);
      } else {
        FilterUtilities.storeSerializedFilter(key, data, currentDeclaration);
      }
    },
    loadFilter: (declaration?: SerializerDeclaration): T => {
      const currentDeclaration = composableDeclaration ?? declaration;
      if (_.isNil(currentDeclaration)) {
        return FilterUtilities.loadFilter(key) as T;
      } else {
        return FilterUtilities.loadSerializedFilter(
          key,
          currentDeclaration
        ) as T;
      }
    },
    clearFilter: (): void => {
      FilterUtilities.clearFilter(key);
    },
  };
}
