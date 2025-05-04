// src/utils/tokenUtils.ts
import Cookies from 'js-cookie';

const TOKEN_KEY = 'jwt';

export const setTokenInCookie = (token: string) => {
  document.cookie = `token=${token}; path=/; max-age=86400`; // 1 day
};

export const getTokenFromCookie = () => {
  const cookies = document.cookie.split(';');
  for (const cookie of cookies) {
    const [name, value] = cookie.trim().split('=');
    if (name === 'token') {
      return value;
    }
  }
  return null;
};

export const removeTokenFromCookie = () => {
  document.cookie = 'token=; path=/; max-age=0';
};

export const decodeToken = () => {
  const token = getTokenFromCookie();
  if (!token) return null;
  try {
    return JSON.parse(atob(token.split('.')[1]));
  } catch (e) {
    return null;
  }
};

export const isTokenExpired = (token: string) => {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp * 1000 < Date.now();
  } catch (error) {
    return true;
  }
};
