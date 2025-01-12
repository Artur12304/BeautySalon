import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import SideMenu from './EmployeeSideMenu';
import { useNavigate } from 'react-router-dom';
import '../styles/EmployeeBookingManagement.css';

const EmployeeBookingManagement: React.FC = () => {
  const [pendingBookings, setPendingBookings] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [confirmationBookingId, setConfirmationBookingId] = useState<
    string | null
  >(null);
  const [isConfirmationVisible, setIsConfirmationVisible] = useState(false);
  const [actionType, setActionType] = useState<'approve' | 'cancel' | null>(
    null,
  );

  const navigate = useNavigate();

  useEffect(() => {
    fetchPendingBookings();
  }, []);

  const fetchPendingBookings = async () => {
    try {
      const currentDate = new Date();
      const startDate = new Date(currentDate.setHours(0, 0, 0, 0));

      const response = await axios.get('/api/bookings/employee', {
        params: {
          startDate: startDate.toISOString(),
          status: 0,
        },
      });

      setPendingBookings(response.data);
    } catch (error) {
      console.error('Failed to fetch pending bookings', error);
      setError('Failed to fetch pending bookings.');
    }
  };

  const formatTime = (date: string) => {
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

  const handleApprove = (bookingId: string) => {
    setConfirmationBookingId(bookingId);
    setActionType('approve');
    setIsConfirmationVisible(true);
  };

  const handleCancel = (bookingId: string) => {
    setConfirmationBookingId(bookingId);
    setActionType('cancel');
    setIsConfirmationVisible(true);
  };

  const handleConfirmAction = async () => {
    if (!confirmationBookingId || !actionType) return;

    const updatedStatus = actionType === 'approve' ? 1 : 2;

    console.log(
      `Sending status: ${updatedStatus} for booking: ${confirmationBookingId}`,
    );

    try {
      await axios.put(
        '/api/bookings/status',
        {
          id: confirmationBookingId,
          status: updatedStatus,
        },
        {
          headers: {
            'Content-Type': 'application/json',
          },
        },
      );

      setPendingBookings((prevBookings) =>
        prevBookings.filter((booking) => booking.id !== confirmationBookingId),
      );
      setIsConfirmationVisible(false);
    } catch (error) {
      console.error(`Failed to ${actionType} booking`, error);
    }
  };

  const handleCancelAction = () => {
    setIsConfirmationVisible(false);
    setConfirmationBookingId(null);
  };

  return (
    <div className="employee-booking-management">
      <SideMenu />
      <div className="employee-booking-management-content">
        <h2>Manage Pending Bookings</h2>
        {error && <p className="error-message">{error}</p>}
        {isConfirmationVisible && confirmationBookingId && (
          <div className="confirmation-popup">
            <p>
              Are you sure you want to{' '}
              {actionType === 'approve' ? 'approve' : 'cancel'} the booking?
            </p>
            <button
              onClick={handleConfirmAction}
              className={
                actionType === 'approve' ? 'confirm-approve' : 'confirm-cancel'
              }
            >
              Confirm
            </button>
            <button onClick={handleCancelAction} className="cancel-button">
              Cancel
            </button>
          </div>
        )}
        <section className="pending-appointments">
          {pendingBookings.length === 0 ? (
            <p>No pending appointments.</p>
          ) : (
            pendingBookings.map((booking) => (
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
                      booking.status === 0 ? 'pending' : ''
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
                <div className="action-buttons">
                  <button
                    className="approve-button"
                    onClick={() => handleApprove(booking.id)}
                  >
                    Approve
                  </button>
                  <button
                    className="cancel-button"
                    onClick={() => handleCancel(booking.id)}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            ))
          )}
        </section>
      </div>
    </div>
  );
};

export default EmployeeBookingManagement;
