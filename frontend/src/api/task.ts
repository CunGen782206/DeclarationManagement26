import http from './http';

export const listTasksApi = () => http.get('/tasks');

export const createTaskApi = (payload: Record<string, unknown>) =>
  http.post('/tasks', payload);

export const updateTaskWindowApi = (id: number, payload: Record<string, unknown>) =>
  http.put(`/tasks/${id}/window`, payload);

export const updateTaskStatusApi = (id: number, payload: Record<string, unknown>) =>
  http.put(`/tasks/${id}/status`, payload);
