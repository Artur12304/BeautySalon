import React, { useState, useEffect } from 'react';
import axios from '../../services/api';
import SideMenu from '../../components/EmployeeSideMenu';
import { useNavigate } from 'react-router-dom';
import '../../styles/EmployeeDashboard.css';

const EmployeeDashboard: React.FC = () => {
  const [upcomingBookings, setUpcomingBookings] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetchBookings();
  }, []);

  const fetchBookings = async () => {
    try {
      const currentDate = new Date();
      const startDate = new Date(currentDate.setUTCHours(0, 0, 0, 0));
      const endDate = new Date(currentDate.setUTCHours(23, 59, 59, 999));

      const response = await axios.get('/api/bookings/employee', {
        params: {
          startDate: startDate.toISOString(),
          endDate: endDate.toISOString(),
        },
      });

      const filteredBookings = response.data.filter(
        (booking: any) => booking.status === 0 || booking.status === 1,
      );

      setUpcomingBookings(filteredBookings);
    } catch (error) {
      console.error('Failed to fetch bookings', error);
    }
  };

  const formatTime = (date: string) => {
    const bookingDate = new Date(date);
    return bookingDate.toLocaleTimeString([], {
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const getStatusText = (status: number) => {
    switch (status) {
      case 0:
        return 'Pending';
      case 1:
        return 'Confirmed';
      case 2:
        return 'Cancelled';
      default:
        return 'Unknown';
    }
  };

  return (
    <div className="employee-dashboard">
      <SideMenu />
      <div className="employee-dashboard-content">
        <h2>
          <span className="today-date">{new Date().toLocaleDateString()}</span>
        </h2>
        <section className="upcoming-appointments">
          <h3>Upcoming Appointments</h3>
          {upcomingBookings.length === 0 ? (
            <p className="no-appointments">
              You have no upcoming appointments.
            </p>
          ) : (
            upcomingBookings.map((booking: any) => (
              <div key={booking.id} className="appointment-card">
                <div className="appointment-header">
                  <div>
                    <h4>{booking.serviceName || 'Not Available'}</h4>
                    <p>
                      <strong>Time:</strong> {formatTime(booking.bookingDate)}
                    </p>
                  </div>
                  <span
                    className={`booking-status ${
                      booking.status === 0
                        ? 'pending'
                        : booking.status === 1
                        ? 'confirmed'
                        : 'cancelled'
                    }`}
                  >
                    {getStatusText(booking.status)}
                  </span>
                </div>
                <div className="appointment-body">
                  <div className="booking-details">
                    <p>
                      <strong>Duration:</strong> {booking.serviceDuration}{' '}
                      minutes
                    </p>
                    <p>
                      <strong>Price:</strong> $
                      {booking.servicePrice || 'Not Available'}
                    </p>
                  </div>
                  <div className="customer-details">
                    <p>
                      <strong>Customer:</strong> {booking.customerFirstName}{' '}
                      {booking.customerLastName}
                    </p>
                    <p>
                      <strong>Phone:</strong>{' '}
                      {booking.customerPhoneNumber || 'Not Available'}
                    </p>
                  </div>
                </div>
              </div>
            ))
          )}
        </section>
      </div>
    </div>
  );
};

export default EmployeeDashboard;
