import http from './http';

export const listTasksApi = () => http.get('/tasks');
