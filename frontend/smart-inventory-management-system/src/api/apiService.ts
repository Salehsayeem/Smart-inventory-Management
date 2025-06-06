import { ApiResponse, CreateOrUpdateShopDto, CreateUserRequest, KeyValuePairDto, PermissionDetails, Permissions, ProfileType, RegisteredShops, UpdatePermissionsRequest, UpdateProfileRequest } from '../types';
import { getUserIdFromToken, removeTokenFromCookie ,getSelectedShopFromCookie} from '../utils/cookieUtils';
import { showError, showSuccess } from '../utils/toastService';
import axios from './axiosConfig';

const handleResponse = async <T>(promise: Promise<any>): Promise<T> => {
  try {
    const apiResult = await promise;
    const response: ApiResponse<T> = apiResult.data;
    if (response.statusCode >= 200 && response.statusCode < 300) {
      if (response.message && response.message.trim() !== "") {
        showSuccess(response.message);
      }
      return response.data;
    } else {
      if (response.message && response.message.trim() !== "") {
        showError(response.message);
      }
      return Promise.reject(response.message);
    }
  } catch (error: any) {
    const message = error?.response?.data?.message || 'Server Error';
    if (message && message.trim() !== "") {
      showError(message);
    }

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

//#region Authentication APIs
export const getUserProfile = async (userId: string): Promise<ProfileType> => {
  return await apiService.get<ProfileType>(`/auth/profile?userId=${encodeURIComponent(userId)}`);
};


export const updateUserProfile = async (
  data: UpdateProfileRequest
): Promise<string> => {
  return await apiService.put<string, UpdateProfileRequest>(
    "/auth/updateProfile",
    data
  );
};

export const getAllUserOfShop = async (): Promise<ProfileType[]> => {
  const shopId = getSelectedShopFromCookie();
  const userId = getUserIdFromToken();
  return await apiService.get<ProfileType[]>(
    `/auth/GetAllUsersByShopId`,
    { shopId, userId }
  );
};
export const getAvailableRoles = async (): Promise<KeyValuePairDto[]> => {
  return await apiService.get<KeyValuePairDto[]>(
    `/auth/getAvailableRoles`
  );
};
export const createUser = async (
  data: CreateUserRequest
): Promise<string> => {
  data.shopId = getSelectedShopFromCookie();
  return await apiService.post<string, CreateUserRequest>(
    "/auth/createUser",
    data
  );
};

export const deleteUser = async (
  userId:string,
): Promise<string> => {
  return await apiService.delete<string>(
    `/auth/deleteUser?userId=${encodeURIComponent(userId)}`
  );
};
export const getPermissionsOfUser = async (userId:string): Promise<PermissionDetails[]> => {
  const data =  await apiService.get<Permissions>(`/auth/getPermissionsOfUser`, { userId });
  return data.permissionDetails;
};

export const updatePermissionsOfUser = async (
  userId:string,
  data: UpdatePermissionsRequest[]
): Promise<string> => {
  return await apiService.put<string, UpdatePermissionsRequest[]>(
    `/auth/updatePermissionsOfUser?userId=${encodeURIComponent(userId)}`,
    data
  );
};

//#endregion

//#region Shop APIs

export const getShopById = async (shopId: number): Promise<RegisteredShops> => {
  const userId = getUserIdFromToken();
  return await apiService.get<RegisteredShops>(`/shop/getShopById`, { userId, shopId });
};
export const createOrUpdateShop = async (
  data: CreateOrUpdateShopDto
): Promise<string> => {

  return await apiService.post<string, CreateOrUpdateShopDto>(
    '/shop/createOrUpdateShop',
    data
  );
};

export const deleteShop = async (shopId: number): Promise<string> => {
  return await apiService.put<string, number>(`/shop/deleteShop`, shopId);
};


//#endregion