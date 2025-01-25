import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import CustomerSideMenu from '../components/CustomerSideMenu';
import { useLocation, useNavigate } from 'react-router-dom';
import '../styles/Booking.css';
const Booking: React.FC = () => {
  const [services, setServices] = useState<any[]>([]);
  const [selectedService, setSelectedService] = useState<string | null>(null);
  const [bookings, setBookings] = useState<any[]>([]);
  const [isCalendarVisible, setIsCalendarVisible] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const queryParams = new URLSearchParams(location.search);
    const serviceId = queryParams.get('serviceId');
    const calendarVisible = queryParams.get('calendarVisible');

    setSelectedService(serviceId);
    setIsCalendarVisible(calendarVisible === 'true');

    fetchServices();
    fetchBookings();
  }, [location]);

  const fetchServices = async () => {
    try {
      const response = await axios.get('/api/services');
      setServices(response.data);
    } catch (error) {
      console.error('Failed to fetch services', error);
    }
  };

  const fetchBookings = async () => {
    try {
      const response = await axios.get('/api/bookings/employee');
      setBookings(response.data);
    } catch (error) {
      console.error('Failed to fetch bookings', error);
    }
  };

  const handleDateClick = (info: any) => {
    const selectedDate = info.dateStr.split('T')[0];
    navigate(`/calendar/${selectedDate}?serviceId=${selectedService}`);
  };

  return (
    <div className="booking-container">
      <CustomerSideMenu />
      <h2>Book Appointment</h2>

      <div className="service-selection">
        <select
          onChange={(e) => setSelectedService(e.target.value)}
          value={selectedService || ''}
        >
          <option value="">Select a Service</option>
          {services.map((service) => (
            <option key={service.id} value={service.id}>
              {service.name} - {service.duration} min
            </option>
          ))}
        </select>
      </div>

      <button
        onClick={() => setIsCalendarVisible(!isCalendarVisible)}
        className={`book-appointment-button ${
          selectedService ? 'active' : 'inactive'
        }`}
        disabled={!selectedService}
      >
        {isCalendarVisible ? 'Hide Calendar' : 'Book Appointment'}
      </button>

      {isCalendarVisible && (
        <div className="fullcalendar-container">
          <FullCalendar
            plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
            initialView="dayGridMonth"
            events={bookings.map((booking) => ({
              id: booking.id,
              title: `${booking.serviceName} with ${booking.customerName}`,
              start: booking.bookingDate,
              backgroundColor: booking.status === 'Completed' ? 'green' : 'red',
            }))}
            dateClick={handleDateClick}
            headerToolbar={{
              left: 'prev,next today',
              center: 'title',
              right: 'dayGridMonth,timeGridWeek,timeGridDay',
            }}
            validRange={{
              start: new Date().toISOString().split('T')[0],
            }}
          />
        </div>
      )}
    </div>
  );
};

export default Booking;
