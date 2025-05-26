
export interface JwtPayload {
  sub: string;
  email: string;
  name: string;
  Shops: RegisteredShops[];
  Role: string;
  Permissions: Permissions;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
export interface Permissions {
  userId: string;
  roleId: number;
  roleName: string;
  permissionDetails: any[];
}

export interface PermissionDetails {
  id: number;
  moduleId: number;
  moduleName: string;
  menuIcon: string;
  path: string;
  isCreate: boolean;
  isView: boolean;
  isEdit: boolean;
  isList: boolean;
  isDelete: boolean;
  isActive: boolean;
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
export interface RegisteredShops {
  id: number;
  name: string;
  address: string;
}
export interface ProfileType {
  id: string;
  fullName: string;
  email: string;
  roleName: string;
  registeredShops: RegisteredShops[];
}
export interface UpdateProfileRequest {
  id: string;
  fullName: string;
}
export interface CreateOrUpdateShopDto{
  id: number;
  name: string;
  address: string;
}
export interface ApiResponse<T> {
  statusCode: number;
  message: string;
  data: T;
}