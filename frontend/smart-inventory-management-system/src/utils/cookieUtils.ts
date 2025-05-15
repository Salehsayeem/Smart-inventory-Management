import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload, PermissionDetails, Permissions } from '../types';

export const setTokenInCookie = (token: string) => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    const expirationDate = new Date(decoded.exp * 1000);
    Cookies.set('token', token, { expires: expirationDate });
    Cookies.set('permissions', JSON.stringify(decoded.Permissions), { expires: expirationDate });
  } catch (error) {
    console.error('Error setting token in cookie:', error);
  }
};

export const getTokenFromCookie = () => {
  const token = Cookies.get('token');
  return token;
};

export const getPermissionsFromCookie = (): Permissions | undefined => {
  try {
    const permissions = Cookies.get('permissions');
    return permissions ? JSON.parse(permissions) as Permissions : undefined;
  } catch (error) {
    console.error('Error parsing permissions from cookie:', error);
    return undefined;
  }
};

export const removeTokenFromCookie = () => {
  Cookies.remove('token');
  Cookies.remove('permissions');
  Cookies.remove('user_info');
};

export const isTokenExpired = (token: string): boolean => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    const currentTime = Date.now() / 1000;
    return decoded.exp < currentTime;
  } catch {
    return true;
  }
};

export const checkTokenExpiration = () => {
  const token = getTokenFromCookie();
  if (!token || isTokenExpired(token)) {
    removeTokenFromCookie();
    return false;
  }
  return true;
};
