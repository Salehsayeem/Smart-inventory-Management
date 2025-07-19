
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
export interface UpdatePermissionsRequest {
  id: number;
  isCreate: boolean;
  isView: boolean;
  isEdit: boolean;
  isList: boolean;
  isDelete: boolean;
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
export interface CreateUserRequest {
  fullName: string;
  email: string;
  roleId: number;
  shopId: number;
}
export interface KeyValuePairDto {
  key: number;
  value: string;
}
export interface UpdateProfileRequest {
  id: string;
  fullName: string;
}
export interface CreateOrUpdateShopDto {
  id: number;
  name: string;
  address: string;
}
export interface CreateOrUpdateProductDto {
  id: number;
  shopId: number;
  categoryId: number;
  name: string;
  sku: string;
  description: string;
  unitPrice: string;
}
export interface ProductLandingDataDto {
  sl: number;
  id: number;
  name: string;
  sku: string;
  description: string;
  categoryName?: string;
  unitPrice: number;
}

export interface ApiResponse<T> {
  statusCode: number;
  message: string;
  data: T;
}