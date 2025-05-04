import React from 'react';
import './FileManager.css';

const FileManager: React.FC = () => {
  return (
    <div className="file-manager-container">
      <h1>File Manager</h1>
      <div className="file-grid">
        <div className="file-card">
          <div className="file-icon">
            <i className="bx bxs-file-pdf"></i>
          </div>
          <div className="file-info">
            <h3>Project Proposal.pdf</h3>
            <p className="file-size">2.4 MB</p>
          </div>
          <div className="file-actions">
            <button className="action-btn">
              <i className="bx bx-download"></i>
            </button>
            <button className="action-btn">
              <i className="bx bx-trash"></i>
            </button>
          </div>
        </div>
        <div className="file-card">
          <div className="file-icon">
            <i className="bx bxs-file-doc"></i>
          </div>
          <div className="file-info">
            <h3>Meeting Notes.docx</h3>
            <p className="file-size">1.8 MB</p>
          </div>
          <div className="file-actions">
            <button className="action-btn">
              <i className="bx bx-download"></i>
            </button>
            <button className="action-btn">
              <i className="bx bx-trash"></i>
            </button>
          </div>
        </div>
        <div className="file-card">
          <div className="file-icon">
            <i className="bx bxs-file-image"></i>
          </div>
          <div className="file-info">
            <h3>Project Screenshot.png</h3>
            <p className="file-size">3.2 MB</p>
          </div>
          <div className="file-actions">
            <button className="action-btn">
              <i className="bx bx-download"></i>
            </button>
            <button className="action-btn">
              <i className="bx bx-trash"></i>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FileManager; 