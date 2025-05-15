export interface JwtPayload {
  sub: string;
  email: string;
  name: string;
  Shop: any[];
  Role: string;
  Permissions: Permissions;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
export interface Permissions {
  UserId: string;
  RoleId: number;
  RoleName: string;
  PermissionDetails: any[];
}

export interface PermissionDetails {
  Id: number;
  ModuleId: number;
  ModuleName: string;
  MenuIcon: string;
  Path: string;
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
  Shop: any[];
  Role: string;
  Permissions: Permissions;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
export interface SignupRequest {
  email: string;
  password: string;
  confirmPassword: string;
  businessName: string;
}
export interface ApiResponse<T> {
  statusCode: number;
  message: string;
  data: T;
}