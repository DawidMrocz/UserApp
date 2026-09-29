import { format, parse } from 'date-fns';
import { pl } from 'date-fns/locale';
import _ from 'lodash';

export default {
  isCorrectDate(obj: unknown): obj is Date {
    return _.isDate(obj) && !_.isNaN(obj.getTime());
  },
  formatDateTime(date: unknown): string | undefined {
    if (_.isString(date)) {
      date = new Date(date);
    }

    if (this.isCorrectDate(date)) {
      return format(date, 'yyyy-MM-dd HH:mm', { locale: pl });
    }
  },
  formatDate(date: unknown): string | undefined {
    if (_.isString(date)) {
      date = new Date(date);
    }

    if (this.isCorrectDate(date)) {
      return format(date, 'yyyy-MM-dd', { locale: pl });
    }
  },
  createCorrectDate(
    year: number,
    month: number,
    day: number
  ): Date | undefined {
    const dateText = `${year}-${month}-${day}`;
    const date = parse(dateText, 'yyyy-MM-dd', new Date());
    return this.isCorrectDate(date) ? date : undefined;
  },
};
