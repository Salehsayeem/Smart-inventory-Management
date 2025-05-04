import React from 'react';
import './AccountManager.css';

const AccountManager: React.FC = () => {
  return (
    <div className="account-manager-container">
      <h1>Account Manager</h1>
      <div className="account-grid">
        <div className="account-card">
          <div className="account-header">
            <img
              src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
              alt="User"
              className="account-avatar"
            />
            <div className="account-info">
              <h3>John Doe</h3>
              <p className="account-email">john.doe@example.com</p>
            </div>
          </div>
          <div className="account-details">
            <div className="detail-item">
              <span className="detail-label">Role</span>
              <span className="detail-value">Administrator</span>
            </div>
            <div className="detail-item">
              <span className="detail-label">Status</span>
              <span className="detail-value active">Active</span>
            </div>
            <div className="detail-item">
              <span className="detail-label">Last Login</span>
              <span className="detail-value">2 hours ago</span>
            </div>
          </div>
        </div>
        <div className="account-card">
          <div className="account-header">
            <img
              src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
              alt="User"
              className="account-avatar"
            />
            <div className="account-info">
              <h3>Jane Smith</h3>
              <p className="account-email">jane.smith@example.com</p>
            </div>
          </div>
          <div className="account-details">
            <div className="detail-item">
              <span className="detail-label">Role</span>
              <span className="detail-value">Editor</span>
            </div>
            <div className="detail-item">
              <span className="detail-label">Status</span>
              <span className="detail-value active">Active</span>
            </div>
            <div className="detail-item">
              <span className="detail-label">Last Login</span>
              <span className="detail-value">1 day ago</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountManager; 