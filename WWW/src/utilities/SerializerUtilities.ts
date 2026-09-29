import { format, startOfDay } from 'date-fns';
import _ from 'lodash';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
type SerializableObject = Record<string, any>;
type SerializeType =
  | 'date'
  | 'datetime'
  | 'float'
  | 'integer'
  | 'boolean'
  | 'percentage';

export interface SerializeField {
  name: string;
  type: SerializeType;
}

export type SerializerDeclaration = SerializeField[];

export default {
  deserializeObject<T extends SerializableObject>(
    obj: T,
    fields: SerializerDeclaration
  ): T {
    const result = _.clone(obj);
    fields.forEach((field) => {
      let fieldResult: unknown = _.get(result, field.name);
      if (!_.isNil(fieldResult)) {
        switch (field.type) {
          case 'date':
            if (_.isString(fieldResult)) {
              fieldResult = startOfDay(new Date(fieldResult));
            }
            break;
          case 'datetime':
            if (_.isString(fieldResult)) {
              fieldResult = new Date(fieldResult);
            }
            break;
          case 'float':
            if (_.isString(fieldResult)) {
              fieldResult = parseFloat(fieldResult.replace(',', '.'));
            }
            break;
          case 'integer':
            if (_.isString(fieldResult)) {
              fieldResult = parseInt(fieldResult);
            }
            break;
          case 'boolean':
            if (_.isString(fieldResult)) {
              fieldResult =
                fieldResult.toLowerCase() === 'true' || fieldResult === '1';
            }
            break;
          case 'percentage':
            if (_.isString(fieldResult)) {
              fieldResult = parseFloat(fieldResult.replace(',', '.'));
            }
            if (_.isNumber(fieldResult)) {
              fieldResult = fieldResult * 100;
            }
            break;
        }
        _.set(result, field.name, fieldResult);
      }
    });
    return result;
  },

  deserializeArray<T extends SerializableObject>(
    obj: T[],
    fields: SerializerDeclaration
  ): T[] {
    return obj.map((row) => this.deserializeObject(row, fields));
  },

  serializeObject<T extends SerializableObject>(
    obj: T,
    fields: SerializerDeclaration
  ): T {
    const result = _.clone(obj);
    fields.forEach((field) => {
      let fieldResult: unknown = _.get(result, field.name);
      if (!_.isNil(fieldResult)) {
        switch (field.type) {
          case 'date':
            if (_.isDate(fieldResult) && !isNaN(fieldResult.getTime())) {
              fieldResult = format(fieldResult, 'yyyy-MM-dd');
            }
            break;
          case 'datetime':
            if (_.isDate(fieldResult) && !isNaN(fieldResult.getTime())) {
              fieldResult = format(
                fieldResult,
                "yyyy-MM-dd'T'HH:mm:ss"
              ) as T[keyof T];
            }
            break;
          case 'float':
            if (_.isString(fieldResult)) {
              fieldResult = parseFloat(fieldResult.replace(',', '.'));
            }
            break;
          case 'integer':
            if (_.isString(fieldResult)) {
              fieldResult = parseInt(fieldResult);
            }
            break;
          case 'boolean':
            if (_.isString(fieldResult)) {
              fieldResult =
                fieldResult.toLowerCase() === 'true' || fieldResult === '1';
            }
            break;
          case 'percentage':
            if (_.isString(fieldResult)) {
              fieldResult = parseFloat(fieldResult.replace(',', '.'));
            }
            if (_.isNumber(fieldResult)) {
              fieldResult = fieldResult / 100;
            }
            break;
        }
        _.set(result, field.name, fieldResult);
      }
    });
    return result;
  },
};
