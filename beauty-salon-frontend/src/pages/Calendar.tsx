import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import SideMenu from '../components/EmployeeSideMenu';
import '../styles/Calendar.css';

const Calendar: React.FC = () => {
  const [bookings, setBookings] = useState<any[]>([]);

  useEffect(() => {
    fetchBookings();
  }, []);

  const fetchBookings = async () => {
    try {
      const response = await axios.get('/api/bookings/employee');
      setBookings(response.data);
    } catch (error) {
      console.error('Failed to fetch bookings', error);
    }
  };

  const handleDateClick = (info: any) => {
    const selectedDate = info.dateStr;
    info.view.calendar.changeView('timeGridDay', selectedDate);
  };

  return (
    <div className="calendar-container">
      <SideMenu />
      <div className="calendar-content">
        <h2>Employee Calendar</h2>
        <FullCalendar
          plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
          initialView="dayGridMonth"
          events={bookings.map((booking) => {
            const startDate = new Date(booking.bookingDate);
            const endDate = new Date(startDate);
            endDate.setMinutes(endDate.getMinutes() + booking.serviceDuration);

            return {
              title: `${booking.serviceName}`,
              start: startDate,
              end: endDate,
              backgroundColor: booking.status === 1 ? 'blue' : 'green',
              borderColor: booking.status === 1 ? 'blue' : 'green',
              extendedProps: {
                phone: booking.customerPhoneNumber,
              },
            };
          })}
          dateClick={handleDateClick}
          headerToolbar={{
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay',
          }}
          firstDay={1}
          slotMinTime="08:00:00"
          slotMaxTime="18:00:00"
          timeZone="local"
          allDaySlot={false}
          slotLabelFormat={{
            hour: '2-digit',
            minute: '2-digit',
            hour12: false,
          }}
        />
      </div>
    </div>
  );
};

export default Calendar;
