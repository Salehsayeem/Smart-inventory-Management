import React from 'react';
import './GroupManager.css';

const GroupManager: React.FC = () => {
  return (
    <div className="group-manager-container">
      <h1>Group Manager</h1>
      <div className="group-grid">
        <div className="group-card">
          <div className="group-header">
            <h3>Development Team</h3>
            <span className="group-status active">Active</span>
          </div>
          <div className="group-members">
            <div className="member-list">
              <div className="member-item">
                <img
                  src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
                  alt="Member"
                  className="member-avatar"
                />
                <span className="member-name">John Doe</span>
              </div>
              <div className="member-item">
                <img
                  src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
                  alt="Member"
                  className="member-avatar"
                />
                <span className="member-name">Jane Smith</span>
              </div>
            </div>
            <span className="member-count">8 members</span>
          </div>
        </div>
        <div className="group-card">
          <div className="group-header">
            <h3>Design Team</h3>
            <span className="group-status active">Active</span>
          </div>
          <div className="group-members">
            <div className="member-list">
              <div className="member-item">
                <img
                  src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
                  alt="Member"
                  className="member-avatar"
                />
                <span className="member-name">Mike Johnson</span>
              </div>
              <div className="member-item">
                <img
                  src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
                  alt="Member"
                  className="member-avatar"
                />
                <span className="member-name">Sarah Wilson</span>
              </div>
            </div>
            <span className="member-count">6 members</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default GroupManager; 