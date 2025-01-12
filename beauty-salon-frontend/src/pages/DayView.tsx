import React, { useEffect, useState } from 'react';
import { useParams, useLocation, useNavigate } from 'react-router-dom';
import axios from '../services/api';
import '../styles/DayView.css';

const DayView: React.FC = () => {
  const { date } = useParams<{ date: string }>();
  const [timeSlots, setTimeSlots] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);
  const location = useLocation();
  const navigate = useNavigate();
  const [selectedSlot, setSelectedSlot] = useState<string | null>(null);
  const [selectedService, setSelectedService] = useState<string | null>(null);
  const [selectedServiceName, setSelectedServiceName] = useState<string | null>(
    null,
  );
  const [serviceDuration, setServiceDuration] = useState<number | null>(null);
  const [serviceCost, setServiceCost] = useState<number | null>(null);
  const [serviceDescription, setServiceDescription] = useState<string | null>(
    null,
  );

  useEffect(() => {
    if (date) {
      const queryParams = new URLSearchParams(location.search);
      const serviceId = queryParams.get('serviceId');
      setSelectedService(serviceId);

      if (serviceId) {
        fetchServiceName(serviceId);
        fetchAvailableSlots(date, serviceId);
      } else {
        setError('Service not selected or customer not authenticated.');
      }
    } else {
      setError('Invalid date.');
    }
  }, [date, location]);

  const fetchAvailableSlots = async (
    date: string,
    serviceId: string | null,
  ) => {
    if (!serviceId) {
      return;
    }
    try {
      const response = await axios.get(`/api/bookings/available-slots`, {
        params: { date, serviceId },
      });
      setTimeSlots(response.data);
    } catch (err) {
      console.error('Failed to load available time slots.');
      setError('Failed to load available time slots.');
    }
  };

  const fetchServiceName = async (serviceId: string | null) => {
    if (!serviceId) {
      return;
    }
    try {
      const response = await axios.get(`/api/services/${serviceId}`);
      setSelectedServiceName(response.data.name);
      setServiceDuration(response.data.duration);
      setServiceCost(response.data.defaultPrice);
      setServiceDescription(response.data.description);
    } catch (err) {
      console.error('Failed to fetch service name.');
      setError('Failed to fetch service name.');
    }
  };

  const handleBookingClick = (time: string) => {
    setSelectedSlot(time);
    navigate(
      `/booking-summary?serviceName=${selectedServiceName}&serviceId=${selectedService}&selectedSlot=${time}&serviceDuration=${serviceDuration}&serviceCost=${serviceCost}&date=${date}`,
    );
  };

  const handleBackToCalendarClick = () => {
    navigate(`/booking?serviceId=${selectedService}&calendarVisible=true`);
  };

  return (
    <div className="day-view">
      <h1>Available Time Slots for {date}</h1>

      {selectedServiceName && <h3>Service: {selectedServiceName}</h3>}

      <div className="service-details">
        {serviceDuration && (
          <div className="service-detail">
            <span>Duration:</span> {serviceDuration} minutes
          </div>
        )}
        {serviceCost && (
          <div className="service-detail">
            <span>Price:</span> ${serviceCost}
          </div>
        )}
      </div>

      {serviceDescription && (
        <div className="service-description">
          <span>Description:</span> {serviceDescription}
        </div>
      )}

      {error && <p className="error-message">{error}</p>}

      <div className="time-slots-container">
        {timeSlots.length === 0 ? (
          <p className="no-slots-message">No available slots for this day.</p>
        ) : (
          timeSlots.map((slot, index) => (
            <div
              key={index}
              className="time-slot available"
              onClick={() => handleBookingClick(slot)}
            >
              <p>{slot}</p>
            </div>
          ))
        )}
      </div>

      <button className="back-button" onClick={handleBackToCalendarClick}>
        Back to Calendar
      </button>
    </div>
  );
};

export default DayView;
