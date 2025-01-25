import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/LandingPage.css';

const LandingPage: React.FC = () => {
  return (
    <div className="landing-page">
      <div className="logo-container">
        <img src="/logo.png" alt="Beauty Salon Logo" className="logo" />
      </div>
      <h1>Welcome to Beauty Salon</h1>
      <p>Your perfect destination for beauty and relaxation.</p>
      <div className="cta-buttons">
        <Link to="/login" className="cta-button">
          Login
        </Link>
        <Link to="/register" className="cta-button">
          Register
        </Link>
      </div>
    </div>
  );
};

export default LandingPage;
