import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import '../styles/EmployeeSideMenu.css';

const AdminSideMenu: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div className="side-menu">
      <h2>Beauty Salon</h2>
      <h5>Admin Dashboard</h5>
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
            to="/manage-employees"
            className={
              location.pathname === '/manage-employees' ? 'active' : ''
            }
          >
            Manage Employees
          </Link>
        </li>
        <li>
          <Link
            to="/admin-services"
            className={location.pathname === '/admin-services' ? 'active' : ''}
          >
            Manage Services
          </Link>
        </li>
        <li>
          <Link
            to="/admin-booking-management"
            className={
              location.pathname === '/admin-booking-management' ? 'active' : ''
            }
          >
            Manage Bookings
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

export default AdminSideMenu;
