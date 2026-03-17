import axios from 'axios';
import { ElMessage } from 'element-plus';
import type { ApiResponse } from '@/types/common';

const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
  timeout: 15000
});

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

http.interceptors.response.use(
  (response) => {
    if (response.data && typeof response.data.success === 'boolean') {
      const body = response.data as ApiResponse<unknown>;
      if (!body.success) {
        ElMessage.error(body.message || '请求失败');
        return Promise.reject(body.message);
      }
    }
    return response;
  },
  (error) => {
    ElMessage.error(error?.response?.data?.message ?? '网络错误');
    return Promise.reject(error);
  }
);

export default http;
