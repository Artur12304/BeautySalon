import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import '../styles/BookingHistory.css';
import EmployeeSideMenu from './EmployeeSideMenu';

interface Booking {
  id: string;
  bookingDate: string;
  serviceName: string;
  customerFirstName: string;
  customerLastName: string;
  customerPhoneNumber: string;
  status: number;
  serviceDuration: number;
  servicePrice: number;
}

const EmployeeBookingHistory: React.FC = () => {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchBookings();
  }, []);

  const fetchBookings = async () => {
    try {
      const today = new Date();
      const endDate = new Date(today.setDate(today.getDate() - 1));

      const response = await axios.get('/api/bookings/employee', {
        params: {
          endDate: endDate.toISOString(),
        },
      });

      setBookings(response.data);
    } catch (err) {
      setError('Failed to fetch booking history.');
    }
  };

  const formatDateTime = (date: string) => {
    const bookingDate = new Date(date);
    return bookingDate.toLocaleString([], {
      weekday: 'short',
      year: 'numeric',
      month: 'short',
      day: 'numeric',
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
    <div className="booking-history">
      <EmployeeSideMenu />
      <h2>Your Booking History</h2>
      {error && <p className="error-message">{error}</p>}
      <section className="booking-history-list">
        {bookings.length === 0 ? (
          <p>You have no past bookings.</p>
        ) : (
          bookings.map((booking) => (
            <div key={booking.id} className="appointment-card">
              <div className="appointment-header">
                <div>
                  <h4>{booking.serviceName || 'Not Available'}</h4>
                  <p>
                    <strong>Time:</strong> {formatDateTime(booking.bookingDate)}
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
                    <strong>Duration:</strong> {booking.serviceDuration} minutes
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
  );
};

export default EmployeeBookingHistory;
