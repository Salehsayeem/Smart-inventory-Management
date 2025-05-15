import React from 'react';
import { ClipLoader } from 'react-spinners';
import './Spinner.css';

const Spinner: React.FC = () => {
  return (
    <div className="spinner-backdrop">
      <div className="spinner-container">
        <ClipLoader color="#4f6bed" size={50} />
        <div className="spinner-text">Loading...</div>
      </div>
    </div>
  );
};

export default Spinner; 