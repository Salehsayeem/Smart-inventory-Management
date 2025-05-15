import React from 'react';
import './Profile.css';

const Profile: React.FC = () => {
  return (
    <div className="dashboard-container">
      <h1>Profile</h1>
      <div className="dashboard-grid">
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Personal Information</h3>
            <button className="view-all-btn">Edit</button>
          </div>
          <div className="profile-info">
            <div className="info-item">
              <div className="info-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="info-content">
                <p className="info-label">Full Name</p>
                <p className="info-value">John Doe</p>
              </div>
            </div>
            <div className="info-item">
              <div className="info-icon">
                <i className="bx bx-envelope"></i>
              </div>
              <div className="info-content">
                <p className="info-label">Email</p>
                <p className="info-value">john.doe@example.com</p>
              </div>
            </div>
            <div className="info-item">
              <div className="info-icon">
                <i className="bx bx-phone"></i>
              </div>
              <div className="info-content">
                <p className="info-label">Phone</p>
                <p className="info-value">+1 234 567 890</p>
              </div>
            </div>
          </div>
        </div>

        <div className="dashboard-card">
          <div className="card-header">
            <h3>Account Settings</h3>
          </div>
          <div className="settings-list">
            <div className="settings-item">
              <div className="settings-icon">
                <i className="bx bx-lock-alt"></i>
              </div>
              <div className="settings-content">
                <p className="settings-text">Change Password</p>
                <span className="settings-description">Last changed 30 days ago</span>
              </div>
            </div>
            <div className="settings-item">
              <div className="settings-icon">
                <i className="bx bx-bell"></i>
              </div>
              <div className="settings-content">
                <p className="settings-text">Notification Preferences</p>
                <span className="settings-description">Email notifications enabled</span>
              </div>
            </div>
            <div className="settings-item">
              <div className="settings-icon">
                <i className="bx bx-shield"></i>
              </div>
              <div className="settings-content">
                <p className="settings-text">Security Settings</p>
                <span className="settings-description">Two-factor authentication disabled</span>
              </div>
            </div>
          </div>
        </div>

        <div className="dashboard-card">
          <div className="card-header">
            <h3>Recent Activity</h3>
            <button className="view-all-btn">View All</button>
          </div>
          <div className="activity-list">
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-lock-alt"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">Password changed</p>
                <span className="activity-time">30 days ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-bell"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">Notification settings updated</p>
                <span className="activity-time">45 days ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">Profile information updated</p>
                <span className="activity-time">60 days ago</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile; 