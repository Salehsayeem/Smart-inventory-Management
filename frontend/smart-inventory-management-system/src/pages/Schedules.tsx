import React from 'react';
import './Schedules.css';

const Schedules: React.FC = () => {
  return (
    <div className="schedules-container">
      <h1>Schedules</h1>
      <div className="schedules-grid">
        <div className="schedule-card">
          <div className="schedule-header">
            <h3>Team Meeting</h3>
            <span className="schedule-time">10:00 AM - 11:00 AM</span>
          </div>
          <p className="schedule-description">Weekly team sync and project updates</p>
          <div className="schedule-participants">
            <span className="participant-count">8 participants</span>
          </div>
        </div>
        <div className="schedule-card">
          <div className="schedule-header">
            <h3>Client Call</h3>
            <span className="schedule-time">2:00 PM - 3:00 PM</span>
          </div>
          <p className="schedule-description">Project review with client stakeholders</p>
          <div className="schedule-participants">
            <span className="participant-count">4 participants</span>
          </div>
        </div>
        <div className="schedule-card">
          <div className="schedule-header">
            <h3>Training Session</h3>
            <span className="schedule-time">4:00 PM - 5:30 PM</span>
          </div>
          <p className="schedule-description">New feature training for team members</p>
          <div className="schedule-participants">
            <span className="participant-count">12 participants</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Schedules; 