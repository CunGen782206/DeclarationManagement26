import http from './http';

export const queryStatisticsApi = (payload: Record<string, unknown>) =>
  http.post('/statistics/query', payload);

export const exportExcelApi = (payload: Record<string, unknown>) =>
  http.post('/statistics/export/excel', payload, { responseType: 'blob' });

export const exportArchiveApi = (payload: Record<string, unknown>) =>
  http.post('/statistics/export/archive', payload, { responseType: 'blob' });
