// src/App.tsx
import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { AuthProvider } from './context/AuthContext';
import { LoadingProvider, useLoading } from './context/LoadingContext';
import Spinner from './components/Spinner';
import AppRoutes from './routes/AppRoutes';
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
      <AppRoutes />
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
