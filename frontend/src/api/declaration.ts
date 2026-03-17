import http from './http';
import type { ApiResponse, PagedResult } from '@/types/common';
import type { AttachmentItem, DeclarationDetail, DeclarationItem } from '@/types/domain';

export const getMyDeclarationsApi = (params: Record<string, unknown>) =>
  http.get<ApiResponse<PagedResult<DeclarationItem>>>('/declarations/mine', { params });

export const getDeclarationDetailApi = (id: number) =>
  http.get<ApiResponse<DeclarationDetail>>(`/declarations/${id}`);

export const createDeclarationApi = (payload: Record<string, unknown>) => http.post('/declarations', payload);

export const updateDeclarationApi = (id: number, payload: Record<string, unknown>) =>
  http.put(`/declarations/${id}`, payload);

export const submitDeclarationApi = (payload: Record<string, unknown>) =>
  http.post<ApiResponse<number>>('/declarations/submit', payload);

export const uploadAttachmentApi = (id: number, file: File) => {
  const formData = new FormData();
  formData.append('file', file);
  return http.post(`/declarations/${id}/attachments`, formData);
};

export const uploadTemporaryAttachmentApi = (tempAttachmentKey: string, file: File) => {
  const formData = new FormData();
  formData.append('file', file);
  return http.post<ApiResponse<number>>('/declarations/temp-attachments', formData, {
    params: { tempAttachmentKey }
  });
};

export const getAttachmentsApi = (id: number) =>
  http.get<ApiResponse<AttachmentItem[]>>(`/declarations/${id}/attachments`);

export const getTemporaryAttachmentsApi = (tempAttachmentKey: string) =>
  http.get<ApiResponse<AttachmentItem[]>>('/declarations/temp-attachments', {
    params: { tempAttachmentKey }
  });

export const clearTemporaryAttachmentsApi = (tempAttachmentKey: string) =>
  http.delete<ApiResponse<string>>('/declarations/temp-attachments', {
    params: { tempAttachmentKey }
  });

export const downloadAttachmentApi = (attachmentId: number) =>
  http.get(`/declarations/attachments/${attachmentId}/download`, { responseType: 'blob' });
