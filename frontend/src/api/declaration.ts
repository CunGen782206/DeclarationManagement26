import http from './http';
import type { ApiResponse, PagedResult } from '@/types/common';
import type { DeclarationItem } from '@/types/domain';

export const getMyDeclarationsApi = (params: Record<string, unknown>) =>
  http.get<ApiResponse<PagedResult<DeclarationItem>>>('/declarations/mine', { params });

export const getDeclarationDetailApi = (id: number) => http.get(`/declarations/${id}`);

export const createDeclarationApi = (payload: Record<string, unknown>) => http.post('/declarations', payload);

export const updateDeclarationApi = (id: number, payload: Record<string, unknown>) =>
  http.put(`/declarations/${id}`, payload);

export const submitDeclarationApi = (declarationId: number) =>
  http.post('/declarations/submit', { declarationId });

export const resubmitDeclarationApi = (declarationId: number) =>
  http.post('/declarations/resubmit', { declarationId });

export const uploadAttachmentApi = (id: number, file: File) => {
  const formData = new FormData();
  formData.append('file', file);
  return http.post(`/declarations/${id}/attachments`, formData);
};

export const getAttachmentsApi = (id: number) =>
  http.get(`/declarations/${id}/attachments`);

export const downloadAttachmentApi = (attachmentId: number) =>
  http.get(`/declarations/attachments/${attachmentId}/download`, { responseType: 'blob' });
