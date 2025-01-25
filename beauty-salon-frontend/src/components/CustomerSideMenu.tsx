import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import '../styles/EmployeeSideMenu.css';

const CustomerSideMenu: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div className="side-menu">
      <h2>Beauty Salon</h2>
      <ul>
        <li>
          <Link
            to="/dashboard"
            className={location.pathname === '/dashboard' ? 'active' : ''}
          >
            Dashboard
          </Link>
        </li>
        <li>
          <Link
            to="/customer-services"
            className={
              location.pathname === '/customer-services' ? 'active' : ''
            }
          >
            Services
          </Link>
        </li>
        <li>
          <Link
            to="/booking"
            className={location.pathname === '/booking' ? 'active' : ''}
          >
            Book Appointment
          </Link>
        </li>
        <li>
          <Link
            to="/customer-booking-history"
            className={
              location.pathname === '/customer-booking-history' ? 'active' : ''
            }
          >
            Booking History
          </Link>
        </li>
        <li>
          <button onClick={handleLogout} className="logout-menu">
            Logout
          </button>
        </li>
      </ul>
    </div>
  );
};

export default CustomerSideMenu;
