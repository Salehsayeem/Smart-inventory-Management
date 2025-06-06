import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload, Permissions } from '../types';
import { keysToCamel } from './caseUtils';

export const setTokenInCookie = (token: string) => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    const expirationDate = new Date(decoded.exp * 1000);
    Cookies.set('token', token, { expires: expirationDate });
    Cookies.set('permissions', JSON.stringify(keysToCamel(decoded.Permissions)), { expires: expirationDate });
  } catch (error) {
    console.error('Error setting token in cookie:', error);
  }
};

export const firstShopIdOnLogin = (token: string) => {
  const decoded = jwtDecode<JwtPayload>(token);
  const expirationDate = new Date(decoded.exp * 1000);
  const parsedShops = JSON.parse(keysToCamel(decoded.Shops)) as any;
  Cookies.set('shopId', parsedShops[0].Id, { expires: expirationDate });
}

export const setSelectedShopInCookie = (shopId: number | undefined) => {
   if (shopId !== undefined && shopId !== null) {
    Cookies.set('shopId', String(shopId));
  }
}
export const getSelectedShopFromCookie = (): number => {
  const selectedShop = Cookies.get('shopId');
  return selectedShop ? parseInt(selectedShop, 10) : 0;
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

export const getRoleIdFromCookie = (): number | undefined => {
  try {
    const permissions = getPermissionsFromCookie();
    if (typeof permissions === "string") {
      const parse = JSON.parse(permissions);
      return parse.RoleId;
    }
  } catch (error) {
    console.error('Error parsing permissions from cookie:', error);
    return undefined;
  }
};

export const removeTokenFromCookie = () => {
  Cookies.remove('token');
  Cookies.remove('permissions');
  Cookies.remove('shopId');
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

export const getUserIdFromToken = (): string | null => {
  const token = getTokenFromCookie();
  if (!token) return null;
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    return decoded.sub;
  } catch (error) {
    console.error('Error decoding token:', error);
    return null;
  }
};
