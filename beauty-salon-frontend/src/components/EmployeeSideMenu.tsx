import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import '../styles/EmployeeSideMenu.css';

const EmployeeSideMenu: React.FC = () => {
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
            to="/services"
            className={location.pathname === '/services' ? 'active' : ''}
          >
            Services
          </Link>
        </li>
        <li>
          <Link
            to="/calendar"
            className={location.pathname === '/calendar' ? 'active' : ''}
          >
            Calendar
          </Link>
        </li>
        <li>
          <Link
            to="/employee-booking-management"
            className={
              location.pathname === '/employee-booking-management'
                ? 'active'
                : ''
            }
          >
            Manage Bookings
          </Link>
        </li>
        <li>
          <Link
            to="/employee-booking-history"
            className={
              location.pathname === '/employee-booking-history' ? 'active' : ''
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

export default EmployeeSideMenu;
