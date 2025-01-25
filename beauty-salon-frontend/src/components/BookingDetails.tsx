import React from 'react';

const BookingDetails: React.FC<{ booking: any; onClose: () => void }> = ({
  booking,
  onClose,
}) => {
  return (
    <div className="booking-details">
      <h3>Booking Details</h3>
      <p>
        <strong>Service:</strong> {booking.serviceName}
      </p>
      <p>
        <strong>Customer:</strong> {booking.customerName}
      </p>
      <p>
        <strong>Employee:</strong> {booking.employeeName}
      </p>
      <p>
        <strong>Date:</strong> {new Date(booking.bookingDate).toLocaleString()}
      </p>
      <p>
        <strong>Status:</strong> {booking.status}
      </p>

      <button onClick={onClose}>Close</button>
    </div>
  );
};

export default BookingDetails;
