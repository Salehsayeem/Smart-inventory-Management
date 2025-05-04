import { JwtPayload, PermissionDetail } from '../types';
import { jwtDecode } from 'jwt-decode';

/**
 * Decodes and parses the JWT token
 * @param token - JWT token string
 * @returns Decoded JWT payload
 */
export const decodeJwt = (token: string): JwtPayload => {
  return jwtDecode<JwtPayload>(token);
};

/**
 * Parses the permissions string from JWT into PermissionDetail array
 * @param permissionsString - JSON string containing permissions
 * @returns Array of PermissionDetail objects
 */
export const parsePermissions = (permissionsString: string): PermissionDetail[] => {
  try {
    const parsed = JSON.parse(permissionsString);
    return parsed.PermissionDetails || [];
  } catch (error) {
    console.error('Error parsing permissions:', error);
    return [];
  }
};

/**
 * Gets menu items based on user permissions
 * @param permissions - Array of PermissionDetail objects
 * @returns Array of menu items with their permissions
 */
export const getMenuItems = (permissions: string[]) => {
  const allMenuItems = [
    { id: 1, label: 'Dashboard', icon: 'home' },
    { id: 2, label: 'Analytics', icon: 'pie-chart-alt' },
    { id: 3, label: 'Schedules', icon: 'time' },
    { id: 4, label: 'Account Manager', icon: 'user' },
    { id: 5, label: 'File Manager', icon: 'folder' },
    { id: 6, label: 'Group Manager', icon: 'group' },
  ];

  // In a real app, you would filter menu items based on permissions
  return allMenuItems;
};

/**
 * Checks if user has permission for a specific action
 * @param permissions - Array of PermissionDetail objects
 * @param moduleName - Name of the module
 * @param action - Action to check (create, view, edit, list, delete)
 * @returns boolean indicating if user has permission
 */
export const hasPermission = (
  permissions: PermissionDetail[],
  moduleName: string,
  action: 'create' | 'view' | 'edit' | 'list' | 'delete'
): boolean => {
  const module = permissions.find(p => p.ModuleName === moduleName);
  if (!module) return false;

  switch (action) {
    case 'create': return module.IsCreate;
    case 'view': return module.IsView;
    case 'edit': return module.IsEdit;
    case 'list': return module.IsList;
    case 'delete': return module.IsDelete;
    default: return false;
  }
}; 