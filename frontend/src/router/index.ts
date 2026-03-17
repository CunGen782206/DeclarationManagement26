import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/store/auth';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/login', component: () => import('@/views/auth/LoginView.vue') },
    {
      path: '/',
      component: () => import('@/layout/MainLayout.vue'),
      children: [
        { path: '', redirect: '/declarations' },
        { path: 'declarations', component: () => import('@/views/declaration/DeclarationListView.vue') },
        { path: 'declarations/new', component: () => import('@/views/declaration/DeclarationFormView.vue') },
        { path: 'declarations/:id', component: () => import('@/views/declaration/DeclarationFormView.vue') },
        { path: 'reviews', component: () => import('@/views/review/ReviewView.vue') },
        { path: 'tasks', component: () => import('@/views/task/TaskManagementView.vue') },
        { path: 'statistics', component: () => import('@/views/statistics/StatisticsView.vue') },
        { path: 'users', component: () => import('@/views/user/UserManagementView.vue') }
      ]
    }
  ]
});

router.beforeEach(async (to) => {
  const auth = useAuthStore();
  if (to.path === '/login') {
    return true;
  }

  if (!auth.isAuthenticated) {
    return '/login';
  }

  if (!auth.user) {
    try {
      await auth.fetchMe();
    } catch {
      auth.clearAuth();
      return '/login';
    }
  }

  if ((to.path === '/statistics' || to.path === '/users' || to.path === '/tasks') && !auth.isSuperAdmin) {
    return '/declarations';
  }

  return true;
});

export default router;
