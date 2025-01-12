import React, { useState, useEffect } from 'react';
import AdminSideMenu from '../../components/AdminSideMenu';
import axios from '../../services/api';

const AdminDashboard: React.FC = () => {
  const [stats, setStats] = useState({
    employeesCount: 0,
    servicesCount: 0,
    bookingsCount: 0,
  });
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const response = await axios.get('/api/dashboard/stats');
        setStats(response.data);
      } catch (err) {
        setError('Failed to fetch dashboard stats.');
        console.error(err);
      }
    };

    fetchStats();
  }, []);

  return (
    <div className="admin-dashboard">
      <AdminSideMenu />
      <div className="admin-content">
        <h2>Admin Dashboard</h2>
        <div className="stats">
          <div className="stat-item">
            <h3>Employees</h3>
            <p>{stats.employeesCount} employees</p>
          </div>
          <div className="stat-item">
            <h3>Services</h3>
            <p>{stats.servicesCount} services</p>
          </div>
          <div className="stat-item">
            <h3>Bookings</h3>
            <p>{stats.bookingsCount} bookings</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
