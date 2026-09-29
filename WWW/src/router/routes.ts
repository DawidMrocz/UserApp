import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('pages/Task/Search/SearchPage.vue'),
        meta: {
          guard: 'authorized',
        },
      },
      {
        path: 'login',
        name: 'Login',
        component: () => import('pages/User/LoginPage.vue'),
        meta: {
          guard: 'guest',
        },
      },
      {
        path: 'reset',
        component: () => import('pages/User/ResetPasswordPage.vue'),
      },
      {
        path: 'remind/:token',
        component: () => import('pages/User/RemindPasswordPage.vue'),
      },
      {
        path: 'activate/:code',
        component: () => import('pages/User/AccountActivatedPage.vue'),
      },
    ],
  },
  {
    path: '/tasks',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: '',
        name: 'tasksMain',
        component: () => import('pages/Task/Search/SearchPage.vue'),
        meta: {
          guard: 'authorized',
        },
      },
      {
        path: 'edit/:id',
        name: 'tasksEdit',
        component: () => import('pages/Task/Edit/TaskEditPage.vue'),
        meta: {
          guard: 'authorized',
        },
      },
      {
        path: 'create',
        name: 'tasksCreate',
        component: () => import('pages/Task/Create/TaskCreatePage.vue'),
        meta: {
          guard: 'authorized',
        },
      },
      {
        path: ':id',
        name: 'tasks',
        component: () => import('pages/Task/View/TaskViewPage.vue'),
        meta: {
          guard: 'authorized',
        },
      },
    ],
  },
  {
    path: '/bilboard',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: '',
        name: 'bilboardMain',
        component: () => import('pages/Bilboard/BilboardViewPage.vue'),
        meta: {
          guard: 'authorized',
        },
      },
    ],
  },

  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
