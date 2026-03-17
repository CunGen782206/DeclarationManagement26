import http from './http';
import type { ApiResponse } from '@/types/common';
import type { CurrentUser, LoginResponse } from '@/types/domain';

export const loginApi = (payload: { jobNumber: string; password: string }) =>
  http.post<ApiResponse<LoginResponse>>('/auth/login', payload);

export const meApi = () => http.get<ApiResponse<CurrentUser>>('/auth/me');

export const changePasswordApi = (payload: { oldPassword: string; newPassword: string }) =>
  http.post<ApiResponse<string>>('/auth/change-password', payload);
