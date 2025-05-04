import React from 'react';
import './Dashboard.css';

const Dashboard: React.FC = () => {
  return (
    <div className="dashboard-container">
      <h1>Dashboard</h1>
      <div className="dashboard-grid">
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Recent Activity</h3>
            <button className="view-all-btn">View All</button>
          </div>
          <div className="activity-list">
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">John Doe created a new account</p>
                <span className="activity-time">2 hours ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-file"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">New file uploaded to Project A</p>
                <span className="activity-time">4 hours ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-group"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">New team member joined</p>
                <span className="activity-time">1 day ago</span>
              </div>
            </div>
          </div>
        </div>
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Quick Stats</h3>
          </div>
          <div className="stats-grid">
            <div className="stat-item">
              <div className="stat-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="stat-info">
                <p className="stat-value">1,234</p>
                <p className="stat-label">Total Users</p>
              </div>
            </div>
            <div className="stat-item">
              <div className="stat-icon">
                <i className="bx bx-file"></i>
              </div>
              <div className="stat-info">
                <p className="stat-value">456</p>
                <p className="stat-label">Total Files</p>
              </div>
            </div>
            <div className="stat-item">
              <div className="stat-icon">
                <i className="bx bx-group"></i>
              </div>
              <div className="stat-info">
                <p className="stat-value">12</p>
                <p className="stat-label">Total Groups</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard; 