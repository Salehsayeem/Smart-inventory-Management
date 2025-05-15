import React from 'react';
import './UserManagement.css';

const UserManagement: React.FC = () => {
  return (
    <div className="dashboard-container">
      <h1>User Management</h1>
      <div className="dashboard-grid">
        {/* Statistics Cards */}
        <div className="dashboard-card">
          <div className="card-header">
            <h3>User Statistics</h3>
          </div>
          <div className="statistics-grid">
            <div className="statistic-item">
              <div className="statistic-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="statistic-content">
                <h4>Total Users</h4>
                <p className="statistic-value">1,234</p>
                <span className="statistic-change positive">+12% from last month</span>
              </div>
            </div>
            <div className="statistic-item">
              <div className="statistic-icon">
                <i className="bx bx-user-check"></i>
              </div>
              <div className="statistic-content">
                <h4>Active Users</h4>
                <p className="statistic-value">1,100</p>
                <span className="statistic-change positive">+8% from last month</span>
              </div>
            </div>
            <div className="statistic-item">
              <div className="statistic-icon">
                <i className="bx bx-user-x"></i>
              </div>
              <div className="statistic-content">
                <h4>Inactive Users</h4>
                <p className="statistic-value">134</p>
                <span className="statistic-change negative">+3% from last month</span>
              </div>
            </div>
          </div>
        </div>

        {/* User List */}
        <div className="dashboard-card">
          <div className="card-header">
            <h3>User List</h3>
            <button className="view-all-btn">Add New User</button>
          </div>
          <div className="user-list">
            <div className="user-item">
              <div className="user-avatar">
                <img src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png" alt="User" />
              </div>
              <div className="user-info">
                <h4>John Doe</h4>
                <p>john.doe@example.com</p>
                <span className="user-role">Administrator</span>
              </div>
              <div className="user-status active">Active</div>
              <div className="user-actions">
                <button className="action-btn edit"><i className="bx bx-edit"></i></button>
                <button className="action-btn delete"><i className="bx bx-trash"></i></button>
              </div>
            </div>
            <div className="user-item">
              <div className="user-avatar">
                <img src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png" alt="User" />
              </div>
              <div className="user-info">
                <h4>Jane Smith</h4>
                <p>jane.smith@example.com</p>
                <span className="user-role">Manager</span>
              </div>
              <div className="user-status active">Active</div>
              <div className="user-actions">
                <button className="action-btn edit"><i className="bx bx-edit"></i></button>
                <button className="action-btn delete"><i className="bx bx-trash"></i></button>
              </div>
            </div>
            <div className="user-item">
              <div className="user-avatar">
                <img src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png" alt="User" />
              </div>
              <div className="user-info">
                <h4>Mike Johnson</h4>
                <p>mike.johnson@example.com</p>
                <span className="user-role">Staff</span>
              </div>
              <div className="user-status inactive">Inactive</div>
              <div className="user-actions">
                <button className="action-btn edit"><i className="bx bx-edit"></i></button>
                <button className="action-btn delete"><i className="bx bx-trash"></i></button>
              </div>
            </div>
          </div>
        </div>

        {/* Recent Activities */}
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Recent Activities</h3>
            <button className="view-all-btn">View All</button>
          </div>
          <div className="activity-list">
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-user-plus"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">New user added: Sarah Wilson</p>
                <span className="activity-time">2 hours ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-user-x"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">User deactivated: Mike Johnson</p>
                <span className="activity-time">5 hours ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-edit"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">User role updated: Jane Smith</p>
                <span className="activity-time">1 day ago</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserManagement; 