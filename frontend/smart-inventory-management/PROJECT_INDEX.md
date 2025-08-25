# Smart Inventory Management - Project Index (Updated)

## Project Overview
A modern Angular-based inventory management system with authentication, role-based permissions, and a responsive UI built with Angular Material.

## Technology Stack
- **Framework**: Angular 20.1.0
- **UI Library**: Angular Material 20.1.3
- **Styling**: SCSS with Angular Material theming
- **HTTP Client**: Angular HttpClient with interceptors
- **State Management**: Angular signals and reactive forms
- **Loading**: ngx-ui-loader for loading states
- **Cookies**: ngx-cookie-service for token management
- **Icons**: Font Awesome (fal, bx prefixes)

## Current Implementation Status

### ✅ Working Components
- **App Component**: Has `isAuthRoute()` method to detect auth pages
- **Login Flow**: Redirects to dashboard after successful authentication
- **Side Navigation**: Dynamic navigation based on user permissions
- **Routing Structure**: Proper route organization with auth and main routes
- **Responsive Design**: Sidebar collapses on mobile devices

### ❌ Issues to Fix
1. **Duplicate Sidebar in Main Page**: The main-page component has its own sidebar, causing conflicts
2. **Router Outlet Placement**: Router outlet should be inside main-page for non-auth routes
3. **Component Structure**: Main-page should be a container, not have its own sidebar

## Project Structure

### Root Configuration Files
```
├── package.json              # Dependencies and scripts
├── angular.json              # Angular CLI configuration
├── tsconfig.json             # TypeScript configuration
├── README.md                 # Project documentation
└── .gitignore               # Git ignore rules
```

### Source Code Structure (`src/`)
```
src/
├── main.ts                   # Application entry point
├── index.html               # Main HTML template
├── styles.scss              # Global styles and Material theme
├── environments/
│   └── environment.ts       # Environment configuration (API base URL)
├── public/
│   └── favicon.ico         # Application favicon
└── app/                     # Main application code
```

### Application Architecture (`src/app/`)

#### Core Application Files
- `app.ts` - Main application component with responsive sidebar logic and auth route detection
- `app-module.ts` - Root module with all imports and providers
- `app-routing-module.ts` - Main routing configuration
- `app.html` - Main application template with conditional rendering
- `app.scss` - Application-specific styles

#### Authentication Module (`auth/`)
```
auth/
├── auth-module.ts           # Authentication module configuration
├── auth-routing.module.ts   # Auth routing (login/signup)
├── auth-service.ts          # JWT token management and decoding
├── login/
│   ├── login.ts            # Login component logic with redirect to dashboard
│   ├── login.html          # Login form template
│   └── login.scss          # Login styles
└── signup/
    ├── signup.ts           # Signup component logic
    ├── signup.html         # Signup form template
    └── signup.scss         # Signup styles
```

#### API Layer (`api/`)
```
api/
├── api-service.ts           # HTTP service for API calls
├── api-feedback.interceptor.ts  # HTTP interceptor for feedback
└── schema/
    ├── request/
    │   └── auth.request.ts  # Authentication request interfaces
    └── response/
        ├── auth-response.ts # Authentication response interfaces
        └── api-common.response.ts # Common API response interface
```

#### Features (`features/`)
```
features/
├── features-module.ts       # Features module configuration
├── main-page/              # Main application layout (NEEDS FIXING)
│   ├── main-page.ts        # Main page component with responsive logic
│   ├── main-page.html      # Main page template (has duplicate sidebar)
│   └── main-page.scss      # Main page styles
└── dashboard/              # Dashboard feature
    ├── dashboard.ts        # Dashboard component with logout functionality
    ├── dashboard.html      # Dashboard template
    └── dashboard.scss      # Dashboard styles
```

#### Shared Components (`components/`)
```
components/
├── side-nav/               # Navigation sidebar
│   ├── side-nav.ts         # Sidebar component with dynamic navigation items
│   ├── side-nav.html       # Sidebar template
│   └── side-nav.scss       # Sidebar styles
└── snackbar/               # Notification system
    └── snackbar.service.ts # Snackbar service for user feedback
```

## Current Authentication Flow

### ✅ Working Flow
1. **User lands on `/`** → Redirects to `/auth/login`
2. **Login page** → Shows without sidebar (auth route detected)
3. **After login** → Redirects to `/dashboard`
4. **Dashboard page** → Shows within main-page layout with sidebar

### ❌ Issues in Current Flow
1. **Main Page Duplicate Sidebar**: The main-page component has its own sidebar, causing conflicts
2. **Router Outlet Placement**: Router outlet should be inside main-page for non-auth routes
3. **Component Structure**: Main-page should be a container, not have its own sidebar

## Required Fixes

### 1. Fix Main Page Component (`main-page.html`)
**Current Issue**: Has its own sidebar component
**Solution**: Remove sidebar, keep only the container div

```html
<!-- CURRENT (INCORRECT) -->
<app-side-nav
  [isLeftSidebarCollapsed]="isLeftSidebarCollapsed()"
  (changeIsLeftSidebarCollapsed)="changeIsLeftSidebarCollapsed($event)"
></app-side-nav>
<div class="body" [ngClass]="sizeClass()">
  <ng-content></ng-content>
</div>

<!-- SHOULD BE -->
<div class="body" [ngClass]="sizeClass()">
  <ng-content></ng-content>
</div>
```

### 2. Fix Main Page Component (`main-page.ts`)
**Current Issue**: Has unused methods and properties
**Solution**: Remove unused sidebar-related code

```typescript
// REMOVE THESE UNUSED PROPERTIES/METHODS
isLeftSidebarCollapsedValue: boolean = false;
changeIsLeftSidebarCollapsed(newValue: boolean) {
  this.isLeftSidebarCollapsedValue = newValue;
}
```

### 3. Update App Template Structure
**Current Status**: ✅ Correctly implemented
- Shows auth pages without sidebar
- Shows main app with sidebar for non-auth pages
- Router outlet properly placed inside main-page

## Key Features

### Authentication System
- **JWT Token Management**: Secure token storage in cookies
- **Role-Based Access**: Permission-based navigation and features
- **Token Decoding**: Automatic JWT payload parsing
- **Login/Signup**: Complete authentication flow

### Responsive Design
- **Mobile-First**: Responsive sidebar that collapses on mobile
- **Material Design**: Consistent UI using Angular Material
- **Custom Theme**: Spring green primary color scheme

### API Integration
- **HTTP Interceptors**: Automatic error handling and feedback
- **Loading States**: Global loading indicators
- **Error Handling**: User-friendly error messages via snackbars

### Navigation System
- **Dynamic Sidebar**: Collapsible navigation with icons
- **Route Protection**: Authentication-based routing
- **Permission-Based Menu**: Menu items based on user permissions

## Data Models

### Authentication Response
```typescript
interface AuthResponse {
  sub: string;              // User ID
  email: string;            // User email
  name: string;             // User name
  shops: string;            // JSON string of associated shops
  role: string;             // User role
  permissions: PermissionDto; // User permissions
  nbf: number;              // Not before timestamp
  exp: number;              // Expiration timestamp
  iat: number;              // Issued at timestamp
  iss: string;              // Issuer
  aud: string;              // Audience
}
```

### Permission System
```typescript
interface PermissionDto {
  userId: string;
  roleId: number;
  roleName: string;
  permissionDetails: PermissionDetailsDto[];
}

interface PermissionDetailsDto {
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
```

## Available Modules (Based on Permissions)
1. **Dashboard** - Main overview and analytics
2. **Profile** - User profile management
3. **User Management** - User administration
4. **Products** - Product catalog management
5. **Categories** - Product categorization
6. **Inventory Tracking** - Stock management
7. **Purchase Orders** - Order management
8. **Suppliers** - Supplier management
9. **Reports & Logs** - Analytics and audit trails

## Development Commands
```bash
# Start development server
ng serve

# Build for production
ng build

# Run tests
ng test

# Watch mode for development
ng build --watch --configuration development
```

## Environment Configuration
- **API Base URL**: `https://localhost:7268/api/`
- **Production Mode**: Configured for optimization
- **Development Mode**: Source maps and debugging enabled

## Dependencies

### Core Dependencies
- `@angular/*` - Angular framework packages (v20.1.0)
- `@angular/material` - Material Design components
- `@angular/cdk` - Component Development Kit
- `rxjs` - Reactive programming library
- `zone.js` - Zone.js for change detection

### UI/UX Dependencies
- `ngx-ui-loader` - Loading indicators
- `ngx-cookie-service` - Cookie management
- `@ngneat/until-destroy` - Component lifecycle management

### Development Dependencies
- `@angular/cli` - Angular CLI tools
- `@angular/build` - Build system
- `sass` - SCSS preprocessor
- `typescript` - TypeScript compiler
- `karma` & `jasmine` - Testing framework

## Architecture Patterns

### Component Architecture
- **Standalone Components**: Modern Angular standalone component pattern
- **Feature Modules**: Organized by business features
- **Shared Components**: Reusable UI components

### State Management
- **Angular Signals**: Modern reactive state management
- **Reactive Forms**: Form state management
- **Service Layer**: Business logic encapsulation

### Security
- **JWT Authentication**: Secure token-based authentication
- **Route Guards**: Protected routes based on authentication
- **Permission-Based Access**: Feature access based on user permissions

## Styling Architecture
- **Material Design**: Consistent design system
- **SCSS Variables**: Customizable theme variables
- **Responsive Design**: Mobile-first approach
- **Custom Animations**: Smooth transitions and feedback

## API Integration Patterns
- **Service Layer**: Centralized API communication
- **Interceptor Pattern**: Cross-cutting concerns (loading, errors)
- **Type Safety**: Strongly typed request/response interfaces
- **Error Handling**: Consistent error feedback to users

## Next Steps to Complete Implementation

1. **Fix Main Page Component**: Remove duplicate sidebar from main-page
2. **Clean Up Unused Code**: Remove unused methods and properties
3. **Test Authentication Flow**: Verify login → dashboard flow works correctly
4. **Add Route Guards**: Implement authentication guards for protected routes
5. **Add More Features**: Implement remaining modules (products, categories, etc.)

This project represents a modern, scalable Angular application with enterprise-grade features for inventory management, featuring a clean architecture, responsive design, and robust authentication system. The main issue to resolve is the duplicate sidebar in the main-page component. 