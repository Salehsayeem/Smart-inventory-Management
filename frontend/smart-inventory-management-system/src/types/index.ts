export enum MenuTitle {
    Home = 'Home',
    Pages = 'Pages',
}
export interface AppState {
  isOpen: boolean;
  isMobile: boolean;
}
export interface CommonResponse<T = any> {
    statusCode: number;
    message: string;
    data: T;
  }
  
  
export interface AuthContextProps {
    isAuthenticated: boolean;
    user: {
      email: string;
      name: string;
      role: string;
    } | null;
    permissions: PermissionDetail[];
    login: (token: string) => void;
    logout: () => void;
    loading: boolean;
}
export interface LoginRequest {
    email: string;
    password: string;
}
export interface SideBarProps {
    isOpen: boolean;
    toggle: () => void;
}
export interface NavItem {
    id: string;
    label: string;
    icon?: string;
    link?: string;
    subItems?: NavItem[];
  }
  export interface SidebarProps {
    isOpen: boolean;
    toggleSidebar: () => void;
  }

export interface PermissionDetail {
  Id: number;
  ModuleId: number;
  ModuleName: string;
  IsCreate: boolean;
  IsView: boolean;
  IsEdit: boolean;
  IsList: boolean;
  IsDelete: boolean;
  IsActive: boolean;
}

export interface JwtPayload {
  sub: string;
  email: string;
  name: string;
  Shop: string;
  Role: string;
  Permissions: string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}

export interface LoginResponse {
  statusCode: number;
  message: string;
  data: string; // JWT token
}

export interface ApiResponse<T> {
  statusCode: number;
  message: string;
  data: T;
}