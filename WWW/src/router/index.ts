import { route } from 'quasar/wrappers';
import {
  createMemoryHistory,
  createRouter,
  createWebHashHistory,
  createWebHistory,
} from 'vue-router';

import routes from './routes';
import { useUserStore } from 'src/stores/user';

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Router instance.
 */

declare module 'vue-router' {
  interface RouteMeta {
    containerWidth?: string | 'sm' | 'md' | 'lg' | 'xl' | 'full';
    animation?: string;
    title?: string;
    guard?: 'authorized' | 'guest';
  }
}

export default route(function () {
  const createHistory = process.env.SERVER
    ? createMemoryHistory
    : process.env.VUE_ROUTER_MODE === 'history'
    ? createWebHistory
    : createWebHashHistory;

  const Router = createRouter({
    scrollBehavior: () => ({ left: 0, top: 0 }),
    routes,
    history: createHistory(process.env.VUE_ROUTER_BASE),
  });

  Router.beforeEach(async (to, from, next) => {
    const guardMode = to.meta?.guard as string | undefined;
    const userStore = useUserStore();

    if (guardMode == 'authorized' && !userStore.isLoggedIn()) {
      next({ name: 'Login' });
    } else {
      next();
    }
  });

  return Router;
});
