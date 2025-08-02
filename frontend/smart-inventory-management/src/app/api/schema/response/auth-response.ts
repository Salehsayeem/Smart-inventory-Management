export interface AuthResponse {
  sub: string; // User ID
  email: string; // User email
  name: string; // User name
  shops: string; // JSON string of shops associated with the user
  role: string; // User role
  permissions: PermissionDto;
  nbf: number; // Not before timestamp
  exp: number; // Expiration timestamp
  iat: number; // Issued at timestamp
  iss: string; // Issuer of the token
  aud: string; // Audience of the token
}
export interface PermissionDto{
  userId: string;
  roleId: number;
  roleName: string;
  permissionDetails: PermissionDetailsDto[];
}
 export interface PermissionDetailsDto{
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
export interface RegisteredShopsDto {
  id: number;
  name: string;
  address: string;
}



// {
//   "sub": "01JWE23KC9BJTX232B0QPBV5VE",
//   "email": "string@string.com",
//   "name": "Saleh",
//   "Shops": "[{\"Id\":8,\"Name\":\"business1\",\"Address\":\"\"}]",
//   "Role": "Admin",
//   "Permissions": "{\"UserId\":\"01JWE23KC9BJTX232B0QPBV5VE\",\"RoleId\":1,\"RoleName\":\"\",\"PermissionDetails\":[{\"Id\":33,\"ModuleId\":1,\"ModuleName\":\"Dashboard\",\"MenuIcon\":\"bx bxs-dashboard\",\"Path\":\"dashboard\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":25,\"ModuleId\":2,\"ModuleName\":\"Profile\",\"MenuIcon\":\"bx bxs-user\",\"Path\":\"profile\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":29,\"ModuleId\":3,\"ModuleName\":\"User\",\"MenuIcon\":\"bx bxs-group\",\"Path\":\"user-management\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":26,\"ModuleId\":4,\"ModuleName\":\"Product\",\"MenuIcon\":\"bx bxs-box\",\"Path\":\"product\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":27,\"ModuleId\":5,\"ModuleName\":\"Category\",\"MenuIcon\":\"bx bxs-category\",\"Path\":\"category\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":30,\"ModuleId\":6,\"ModuleName\":\"Inventory\",\"MenuIcon\":\"bx bxs-archive\",\"Path\":\"inventory-tracking\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":31,\"ModuleId\":7,\"ModuleName\":\"Purchase\",\"MenuIcon\":\"bx bxs-purchase-tag\",\"Path\":\"purchase-orders\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":28,\"ModuleId\":8,\"ModuleName\":\"Suppliers\",\"MenuIcon\":\"bx bxs-store\",\"Path\":\"suppliers\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true},{\"Id\":32,\"ModuleId\":9,\"ModuleName\":\"Reports\",\"MenuIcon\":\"bx bxs-bar-chart\",\"Path\":\"reports-logs\",\"IsCreate\":true,\"IsView\":true,\"IsEdit\":true,\"IsList\":true,\"IsDelete\":true,\"IsActive\":true}]}",
//   "nbf": 1754159086,
//   "exp": 1754162686,
//   "iat": 1754159086,
//   "iss": "SecureApi",
//   "aud": "SecureApiUser"
// }
