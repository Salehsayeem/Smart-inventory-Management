import { ApiResponse } from '../types';
import { removeTokenFromCookie } from '../utils/cookieUtils';
import { showError, showSuccess } from '../utils/toastService';
import axios from './axiosConfig';

const handleResponse = async <T>(promise: Promise<any>): Promise<T> => {
  try {
    const response = await promise;
    const data: ApiResponse<T> = response.data;

    if (data.statusCode >= 200 && data.statusCode < 300) {
      showSuccess(data.message);
      return data.data;
    } else {
      showError(data.message);
      return Promise.reject(data.message);
    }
  } catch (error: any) {
    const message = error?.response?.data?.message || 'Server Error';
    showError(message);

    if (error?.response?.status === 401) {
      removeTokenFromCookie();
      window.location.href = '/login';
    }

    return Promise.reject(message);
  }
};

export const apiService = {
  get: <T>(url: string, params?: any) =>
    handleResponse<T>(axios.get(url, { params })),

  post: <T, R>(url: string, data: R) =>
    handleResponse<T>(axios.post(url, data)),

  put: <T, R>(url: string, data: R) =>
    handleResponse<T>(axios.put(url, data)),

  delete: <T>(url: string) =>
    handleResponse<T>(axios.delete(url)),
};
