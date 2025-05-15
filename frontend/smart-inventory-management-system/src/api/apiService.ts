import { ApiResponse } from '../types';
import { removeTokenFromCookie } from '../utils/cookieUtils';
import { showError, showSuccess } from '../utils/toastService';
import axios from './axiosConfig';

const handleResponse = async <T>(promise: Promise<any>): Promise<T> => {
  try {
    const apiResult = await promise;
    const response: ApiResponse<T> = apiResult.data;
    if (response.statusCode >= 200 && response.statusCode < 300) {
      showSuccess(response.message);
      return response.data;
    } else {
      showError(response.message);
      return Promise.reject(response.message);
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
