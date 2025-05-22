// // src/context/AuthContext.tsx
import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { useLoading } from './LoadingContext';
import { showError } from '../utils/toastService';
import { LoginRequest, SignupRequest, JwtPayload,  RegisteredShops } from '../types';
import { apiService } from '../api/apiService';
import { jwtDecode } from 'jwt-decode';
import { setTokenInCookie, getTokenFromCookie, removeTokenFromCookie } from '../utils/cookieUtils';

interface AuthContextType {
  isAuthenticated: boolean;
  isInitialized: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  signUp: (email: string, password: string, confirmPassword: string, businessName: string) => Promise<boolean>;
  logout: () => void;
  userInfo: {
    email: string;
    name: string;
    role: string;
    shops: RegisteredShops[];
  } | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isInitialized, setIsInitialized] = useState(false);
  const [userInfo, setUserInfo] = useState<AuthContextType['userInfo']>(null);
  const { setLoading } = useLoading();

  const validateToken = useCallback((token: string): boolean => {
    try {
      const decoded = jwtDecode<JwtPayload>(token);
      const currentTime = Date.now() / 1000;
      return decoded.exp > currentTime;
    } catch {
      return false;
    }
  }, []);

  const updateAuthState = useCallback((token: string | null) => {
    if (!token) {
      setIsAuthenticated(false);
      setUserInfo(null);
      return;
    }

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      setIsAuthenticated(true);
      setUserInfo({
        email: decoded.email,
        name: decoded.name || '',
        role: decoded.Role,
        shops: decoded.Shops || [],
      });

    } catch (error) {
      console.error('Error in updateAuthState:', error);
      setIsAuthenticated(false);
      setUserInfo(null);
    }
  }, []);

  useEffect(() => {
    const initializeAuth = () => {
      const token = getTokenFromCookie();
      if (token && validateToken(token)) {
        updateAuthState(token);
      } else {
        removeTokenFromCookie();
        updateAuthState(null);
      }
      setIsInitialized(true);
    };

    initializeAuth();
  }, [validateToken, updateAuthState]);

  const handleLogout = useCallback(() => {
    removeTokenFromCookie();
    setIsAuthenticated(false);
    setUserInfo(null);
  }, []);

  const login = async (email: string, password: string) => {
    if (!isInitialized) return false;

    try {
      setLoading(true);
      const loginRequest: LoginRequest = { email, password };
      const response = await apiService.post<string, LoginRequest>(
        '/auth/login',
        loginRequest
      );
      
      const token = response;
      setTokenInCookie(token);
      updateAuthState(token);
      return true;
    } catch (error: any) {
      showError(error.message || 'Login failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const signUp = async (email: string, password: string, confirmPassword: string, businessName: string) => {
    if (!isInitialized) return false;

    try {
      setLoading(true);
      const signupRequest: SignupRequest = { email, password, confirmPassword, businessName };
      const response = await apiService.post<string, SignupRequest>(
        '/auth/register',
        signupRequest
      );
      
      const token = response;
      setTokenInCookie(token);
      updateAuthState(token);
      return true;
    } catch (error: any) {
      showError(error.message || 'Failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  if (!isInitialized) {
    return null;
  }

  return (
    <AuthContext.Provider 
      value={{ 
        isAuthenticated, 
        isInitialized,
        login, 
        signUp, 
        logout: handleLogout,
        userInfo 
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};