import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import PrivateRoute from '../api/PrivateRoute';
import Sidebar from '../components/sidebar/Sidebar';

// Pages
import Login from '../pages/auth/LoginForm';
import SignupForm from '../pages/auth/SignupForm';
import Dashboard from '../pages/Dashboard';
import Profile from '../pages/Profile';
import UserManagement from '../pages/UserManagement';
import Product from '../pages/Product';
import Category from '../pages/Category';
import InventoryTracking from '../pages/InventoryTracking';
import PurchaseOrders from '../pages/PurchaseOrders';
import Suppliers from '../pages/Suppliers';

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* Public Routes */}
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignupForm />} />
      
      {/* Protected Routes */}
      <Route
        path="/dashboard"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <Dashboard />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <Profile />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/user-management"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <UserManagement />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/product"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <Product />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/category"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <Category />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/inventory-tracking"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <InventoryTracking />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/purchase-orders"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <PurchaseOrders />
            </div>
          </PrivateRoute>
        }
      />
      <Route
        path="/suppliers"
        element={
          <PrivateRoute>
            <div className="main-content">
              <Sidebar />
              <Suppliers />
            </div>
          </PrivateRoute>
        }
      />
      
      {/* Redirect root to dashboard */}
      <Route path="/" element={<Navigate to="/dashboard" replace />} />
      
      {/* Catch all route */}
      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  );
};

export default AppRoutes; 