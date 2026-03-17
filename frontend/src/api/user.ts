import http from './http';

export const listUsersApi = () => http.get('/users');
export const createUserApi = (payload: Record<string, unknown>) => http.post('/users', payload);
export const updateUserApi = (id: number, payload: Record<string, unknown>) => http.put(`/users/${id}`, payload);
export const deleteUserApi = (id: number) => http.delete(`/users/${id}`);
export const resetPasswordApi = (id: number) => http.post(`/users/${id}/reset-password`);
