// // src/context/AuthContext.tsx
import React, { createContext, useContext, useState, useEffect } from 'react';
import { getTokenFromCookie, isTokenExpired, removeTokenFromCookie, setTokenInCookie } from '../utils/cookieUtils';
import { apiService } from '../api/apiService';
import { useLoading } from './LoadingContext';
import { showError, showSuccess } from '../utils/toastService';

interface AuthContextType {
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const { setLoading } = useLoading();

  useEffect(() => {
    const token = getTokenFromCookie();
    if (token && !isTokenExpired(token)) {
      setIsAuthenticated(true);
    } else {
      setIsAuthenticated(false);
      removeTokenFromCookie();
    }
  }, []);

  const login = async (email: string, password: string) => {
    try {
      setLoading(true);
      const token = await apiService.post<string, { email: string; password: string }>(
        '/auth/login',
        { email, password }
      );
      setTokenInCookie(token);
      setIsAuthenticated(true);
      showSuccess('Logged in successfully');
      return true;
    } catch (error: any) {
      showError(error.message || 'Login failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    removeTokenFromCookie();
    setIsAuthenticated(false);
    showSuccess('Logged out successfully');
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};