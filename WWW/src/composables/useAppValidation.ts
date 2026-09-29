import { ValidationRule } from 'quasar';
import { parse } from 'date-fns';
import _ from 'lodash';

export type QInputRule = ValidationRule;

export default function () {
  return {
    conditionalRule(condition: boolean, rule: ValidationRule): ValidationRule {
      return condition ? rule : () => true;
    },
    requireRule(): ValidationRule {
      return (v) => !!v || 'Wymagane';
    },
    minElementCountRule(count: number): QInputRule {
      return (v) => {
        return (
          !v || v.length >= count || `Proszę podać co najmniej ${count} opcje`
        );
      };
    },
    minRule(limit: number): QInputRule {
      return (v) =>
        !v || parseFloat(v) >= limit || `Wartość jest za mała (min ${limit})`;
    },
    maxRule(limit: number): QInputRule {
      return (v) =>
        !v || parseFloat(v) <= limit || `Wartość jest za duża (max ${limit})`;
    },
    minLengthRule(length: number): QInputRule {
      return (v) =>
        !v || v.length >= length || `Tekst jest za krótki (min ${length})`;
    },
    maxLengthRule(length: number): QInputRule {
      return (v) =>
        !v || v.length <= length || `Tekst jest za długi (max ${length})`;
    },
    emailRule(): ValidationRule {
      const regex =
        /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return (v) => !v || regex.test(v) || 'Niepoprawny email';
    },
    passwordRepeatRule(password: string | undefined): ValidationRule {
      return (v) => v === password || 'Hasła się nie zgadzają';
    },
    noOldPasswordRule(oldPassword: string | undefined): ValidationRule {
      return (v) => v !== oldPassword || 'Hasło musi być inne niż poprzednie';
    },
    onlyDigitsRule(): QInputRule {
      return (v) => !v || !!/^\d+$/.exec(v) || 'Tylko cyfry';
    },
    timeRule(): QInputRule {
      return (v: string) => {
        if (_.isEmpty(v)) {
          return true;
        }

        const date = parse(v, 'HH:mm', new Date());
        if (
          !/^\d{2}\:\d{2}$/.exec(v) ||
          !(date instanceof Date) ||
          isNaN(date.getTime())
        ) {
          return 'Niepoprawna godzina';
        }
        return true;
      };
    },
    dateRule(): QInputRule {
      return (v: string) => {
        if (_.isEmpty(v)) {
          return true;
        }

        const date = parse(v, 'dd/MM/yyyy', new Date());
        if (
          !/^\d{2}\/\d{2}\/\d{4}$/.exec(v) ||
          !(date instanceof Date) ||
          isNaN(date.getTime())
        ) {
          return 'Niepoprawna data';
        }
        return true;
      };
    },
    maxProductCountRule(count: number): QInputRule {
      return (v) =>
        !v || v <= count || `Maksymalna ilość produktów to ${count}`;
    },
    minProductCountRule(count: number): QInputRule {
      return (v) => !v || v >= count || `Minimalna ilość produktów to ${count}`;
    },
    maxCountRule(count: number): QInputRule {
      return (v) => !v || v <= count || `Maksymalna wartość to ${count}`;
    },
    minCountRule(count: number): QInputRule {
      return (v) => !v || v >= count || `Minimalna wartość to ${count}`;
    },
    phoneRule(): ValidationRule {
      return (v) =>
        !v ||
        /^((00)|\+)?[\d ]{7,15}$/.test(v) ||
        'Niepoprawny numer telefonu komórkowego';
    },
  };
}
