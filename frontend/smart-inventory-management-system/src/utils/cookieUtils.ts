import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload, Permissions } from '../types';
import { keysToCamel } from './caseUtils';

export const setTokenInCookie = (token: string) => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    const expirationDate = new Date(decoded.exp * 1000);
    Cookies.set('token', token, { expires: expirationDate });
    console.log('before keysToCamel', decoded.Permissions);
    console.log('after keysToCamel', keysToCamel<Permissions[]>(decoded.Permissions));
    console.log('after json stringfy keysToCamel', JSON.stringify(keysToCamel<Permissions[]>(decoded.Permissions)));
    Cookies.set('permissions', JSON.stringify(keysToCamel(decoded.Permissions)), { expires: expirationDate });
  } catch (error) {
    console.error('Error setting token in cookie:', error);
  }
};
export const setSelectedShopInCookie = (shopId: number) => {
  Cookies.set('selectedShop', String(shopId));
}
export const getSelectedShopFromCookie = () => {
  const selectedShop = Cookies.get('selectedShop');
  return selectedShop;
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
  Cookies.remove('selectedShop');
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

export const getUserIdFromToken = () : string | null => {
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
