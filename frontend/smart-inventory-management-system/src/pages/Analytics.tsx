import React from 'react';
import './Analytics.css';

const Analytics: React.FC = () => {
  return (
    <div className="analytics-container">
      <h1>Analytics</h1>
      <div className="analytics-grid">
        <div className="analytics-card">
          <h3>Total Users</h3>
          <p className="analytics-value">1,234</p>
          <p className="analytics-change positive">+12% from last month</p>
        </div>
        <div className="analytics-card">
          <h3>Active Sessions</h3>
          <p className="analytics-value">456</p>
          <p className="analytics-change positive">+5% from last month</p>
        </div>
        <div className="analytics-card">
          <h3>Average Time</h3>
          <p className="analytics-value">12m 34s</p>
          <p className="analytics-change negative">-2% from last month</p>
        </div>
        <div className="analytics-card">
          <h3>Bounce Rate</h3>
          <p className="analytics-value">23.4%</p>
          <p className="analytics-change positive">-3% from last month</p>
        </div>
      </div>
    </div>
  );
};

export default Analytics; 