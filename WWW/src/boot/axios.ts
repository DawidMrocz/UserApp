import { boot } from 'quasar/wrappers';
import axios, { AxiosInstance } from 'axios';
import qs from 'qs';

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $axios: AxiosInstance;
  }
}

const api = axios.create({
  baseURL: window.API,
  withCredentials: true,
  paramsSerializer: {
    encode: (value) => qs.parse(value, { allowDots: true }),
    serialize: (params) => qs.stringify(params, { allowDots: true }),
  },
});

api.interceptors.request.use(
  (config) => {
    const csrfToken = localStorage.getItem('csrfToken'); // Pobieramy token

    if (csrfToken) {
      config.headers['X-CSRF-Token'] = csrfToken; // Dodajemy do nagłówków
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default boot(({ app }) => {
  app.config.globalProperties.$axios = axios;
  app.config.globalProperties.$api = api;
});

export { api };
