import { defineStore } from 'pinia';
import { meApi } from '@/api/auth';
import type { CurrentUser } from '@/types/domain';

interface AuthState {
  token: string;
  user: CurrentUser | null;
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    token: localStorage.getItem('token') ?? '',
    user: null
  }),
  getters: {
    isAuthenticated: (state) => Boolean(state.token),
    canReview: (state) => Boolean(state.user && !state.user.isSuperAdmin),
    isSuperAdmin: (state) => Boolean(state.user?.isSuperAdmin)
  },
  actions: {
    setToken(token: string) {
      this.token = token;
      localStorage.setItem('token', token);
    },
    clearAuth() {
      this.token = '';
      this.user = null;
      localStorage.removeItem('token');
    },
    async fetchMe() {
      const res = await meApi();
      this.user = res.data.data;
    }
  }
});
