import axios from 'axios';
import { Notify } from 'quasar';
import { isNavigationFailure } from 'vue-router';

export default function () {
  async function errorHandler(error?: unknown): Promise<void> {
    try {
      console.error('Application error:', error);
      if (axios.isAxiosError(error)) {
        if (error.response?.status === 400 || error.response?.status === 401) {
          const errorsMessages = Object.values(
            error.response.data.error
          ).flat() as string[];
          errorsMessages.forEach((errorMessage) => {
            Notify.create({
              type: 'negative',
              message: errorMessage,
            });
          });
        } else {
          Notify.create({
            type: 'negative',
            message: 'Wystąpił błąd',
          });
        }
        Notify.create({
          type: 'negative',
          message: 'Wystąpił błąd',
        });
      } else if (isNavigationFailure(error)) {
        Notify.create({
          type: 'negative',
          message: 'Nie odnaleziono strony',
        });
      } else {
        Notify.create({
          type: 'negative',
          message: 'Wystąpił błąd',
        });
      }
    } catch (err) {
      Notify.create({
        type: 'negative',
        message: 'Wystąpił błąd',
      });
    }
  }

  return {
    errorHandler,
  };
}
