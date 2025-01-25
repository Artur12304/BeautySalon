import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import '../styles/BookingSummary.css';
import axios from '../services/api';

const BookingSummary: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const queryParams = new URLSearchParams(location.search);
  const serviceId = queryParams.get('serviceId');
  const selectedSlot = queryParams.get('selectedSlot');
  const date = queryParams.get('date');

  const [serviceDetails, setServiceDetails] = useState<any>(null);
  const [customerId, setCustomerId] = useState<string | null>(null);
  const [isConfirmed, setIsConfirmed] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (serviceId) {
      fetchServiceDetails(serviceId);
    } else {
      setError('Service ID is missing.');
    }

    fetchCustomerId();
  }, [serviceId]);

  const fetchServiceDetails = async (serviceId: string) => {
    try {
      const response = await axios.get(`/api/services/${serviceId}`);
      setServiceDetails(response.data);
    } catch (err) {
      setError('Failed to fetch service details.');
    }
  };

  const fetchCustomerId = async () => {
    try {
      const response = await axios.get('/api/customers/profile');
      if (response.status === 200) {
        setCustomerId(response.data.id);
      } else {
        setError('Failed to fetch customer profile');
      }
    } catch (err) {
      setError('Error fetching customer profile');
    }
  };

  const handleChangeTime = () => {
    navigate(`/calendar/${date}?serviceId=${serviceId}`);
  };

  const handleConfirmBooking = async () => {
    try {
      if (!date || !selectedSlot) {
        setError('Please select a valid time slot.');
        return;
      }
      const localDate = new Date(`${date}T${selectedSlot}:00`);
      const bookingDateUtc = localDate.toISOString();

      const bookingData = {
        bookingDate: bookingDateUtc,
        serviceId: serviceId,
        customerId: customerId || localStorage.getItem('customerId'),
      };

      const response = await axios.post('/api/bookings', bookingData);

      if (response.status === 201) {
        setIsConfirmed(true);
      } else {
        setError('There was an issue creating your booking.');
      }
    } catch (error) {
      setError('Failed to create booking.');
    }
  };

  const servicePrice = serviceDetails
    ? serviceDetails.price || serviceDetails.defaultPrice
    : null;

  const handleGoBackToDashboard = () => {
    navigate(`/dashboard`);
  };

  return (
    <div className="booking-summary-wrapper">
      <div className="booking-summary">
        <h3>Reservation Summary</h3>

        {serviceDetails ? (
          <div className="service-details">
            <p>
              <strong>Service:</strong> {serviceDetails.name}
            </p>
            <p>
              <strong>Selected Time:</strong> {selectedSlot}
            </p>
            <p>
              <strong>Duration:</strong> {serviceDetails.duration} minutes
            </p>
            <p>
              <strong>Cost:</strong> {servicePrice}$
            </p>
          </div>
        ) : (
          <p>{error}</p>
        )}

        {isConfirmed ? (
          <div className="booking-confirmed">
            <p>Your appointment has been confirmed!</p>
            <button className="go-back" onClick={handleGoBackToDashboard}>
              Go Back to Dashboard
            </button>
          </div>
        ) : (
          <div className="button-group">
            <button className="change-button" onClick={handleChangeTime}>
              Change Time
            </button>
            <button className="confirm-button" onClick={handleConfirmBooking}>
              Confirm Booking
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default BookingSummary;
