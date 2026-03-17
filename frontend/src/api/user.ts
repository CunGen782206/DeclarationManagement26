import http from './http';

export const listUsersApi = (params?: Record<string, unknown>) => http.get('/users', { params });
export const createUserApi = (payload: Record<string, unknown>) => http.post('/users', payload);
export const updateUserApi = (id: number, payload: Record<string, unknown>) => http.put(`/users/${id}`, payload);
export const deleteUserApi = (id: number) => http.delete(`/users/${id}`);
export const resetPasswordApi = (id: number) => http.post(`/users/${id}/reset-password`);
