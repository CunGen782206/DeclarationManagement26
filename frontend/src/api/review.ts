import http from './http';

export const getPendingReviewsApi = (params: Record<string, unknown>) =>
  http.get('/reviews/pending', { params });

export const reviewActionApi = (payload: Record<string, unknown>) =>
  http.post('/reviews/action', payload);

export const getReviewRecordsApi = (declarationId: number) =>
  http.get(`/reviews/${declarationId}/records`);

export const getFlowLogsApi = (declarationId: number) =>
  http.get(`/reviews/${declarationId}/flow-logs`);
