// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { AuthProvider } from './context/AuthContext';
import { LoadingProvider, useLoading } from './context/LoadingContext';
import PrivateRoute from './components/PrivateRoute';
import Spinner from './components/Spinner';
import Sidebar from './components/sidebar/Sidebar';
import Login from './pages/auth/LoginForm';
import Dashboard from './pages/Dashboard';
import Analytics from './pages/Analytics';
import Schedules from './pages/Schedules';
import AccountManager from './pages/AccountManager';
import FileManager from './pages/FileManager';
import GroupManager from './pages/GroupManager';
import './App.css';

const AppContent: React.FC = () => {
  const { isLoading } = useLoading();

  return (
    <div className="app-container">
      {isLoading && <Spinner />}
      <ToastContainer
        position="top-right"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
      />
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<Login />} />
        
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
          path="/analytics"
          element={
            <PrivateRoute>
              <div className="main-content">
                <Sidebar />
                <Analytics />
              </div>
            </PrivateRoute>
          }
        />
        <Route
          path="/schedules"
          element={
            <PrivateRoute>
              <div className="main-content">
                <Sidebar />
                <Schedules />
              </div>
            </PrivateRoute>
          }
        />
        <Route
          path="/account-manager"
          element={
            <PrivateRoute>
              <div className="main-content">
                <Sidebar />
                <AccountManager />
              </div>
            </PrivateRoute>
          }
        />
        <Route
          path="/file-manager"
          element={
            <PrivateRoute>
              <div className="main-content">
                <Sidebar />
                <FileManager />
              </div>
            </PrivateRoute>
          }
        />
        <Route
          path="/group-manager"
          element={
            <PrivateRoute>
              <div className="main-content">
                <Sidebar />
                <GroupManager />
              </div>
            </PrivateRoute>
          }
        />
        
        {/* Redirect root to dashboard */}
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        
        {/* Catch all route */}
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </div>
  );
};

const App: React.FC = () => {
  return (
    <Router>
      <LoadingProvider>
        <AuthProvider>
          <AppContent />
        </AuthProvider>
      </LoadingProvider>
    </Router>
  );
};

export default App;
