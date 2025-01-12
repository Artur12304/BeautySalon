import React from 'react';

interface Booking {
  id: string;
  serviceName: string;
  customerName: string;
  bookingDate: string;
  status: string;
}

interface Props {
  bookings: Booking[];
}

const UpcomingBookings: React.FC<Props> = ({ bookings }) => {
  return (
    <section className="upcoming-bookings">
      <h3>Upcoming Bookings</h3>
      {bookings.length === 0 ? (
        <p>No upcoming bookings</p>
      ) : (
        bookings.map((booking) => (
          <div key={booking.id} className="booking-item">
            <p>
              <strong>Service:</strong> {booking.serviceName}
            </p>
            <p>
              <strong>Customer:</strong> {booking.customerName}
            </p>
            <p>
              <strong>Time:</strong>{' '}
              {new Date(booking.bookingDate).toLocaleString()}
            </p>
          </div>
        ))
      )}
    </section>
  );
};

export default UpcomingBookings;
