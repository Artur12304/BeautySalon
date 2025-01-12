import React, { useEffect, useState } from 'react';
import axios from '../services/api';
import CustomerDashboard from './dashboard/CustomerDashboard';
import EmployeeDashboard from './dashboard/EmployeeDashboard';
import AdminDashboard from './dashboard/AdminDashboard';
import { useNavigate } from 'react-router-dom';
import '../styles/Dashboard.css';

const Dashboard: React.FC = () => {
  const [role, setRole] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  useEffect(() => {
    const fetchUserRole = async () => {
      try {
        const response = await axios.get('/api/account/me');
        console.log(response.data.role);
        setRole(response.data.role);
      } catch (err) {
        setError('Failed to fetch user data. Please log in again.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserRole();
  }, []);

  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return (
      <div>
        <p>{error}</p>
        <button onClick={handleLogout} className="logout-button">
          Go to Login
        </button>
      </div>
    );
  }

  return (
    <div className="dashboard-container">
      {role === 'Customer' && <CustomerDashboard />}
      {role === 'Employee' && <EmployeeDashboard />}
      {role === 'Administrator' && <AdminDashboard />}
      {!['Customer', 'Employee', 'Administrator'].includes(role || '') && (
        <p>Unknown role. Please contact support.</p>
      )}
    </div>
  );
};

export default Dashboard;
