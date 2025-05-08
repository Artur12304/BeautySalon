# Beauty Salon Application

The **Beauty Salon** application is a reservation management system for beauty salons, allowing users to book services, view their appointment history, and enabling employees and administrators to manage bookings, staff, and services.

## Technologies

### Frontend:
- **React** with **TypeScript**
- **Vite** for building the application
- **FullCalendar** for managing the booking calendar
- Responsive and adaptive to various devices

### Backend:
- **.NET 8** for backend development
- **Entity Framework Core** for database communication
- **PostgreSQL** as the database
- **JWT (JSON Web Tokens)** for authentication and authorization
- **Serilog** for error and user action logging
- **SMTP** for sending email notifications about booking confirmations

## Features

- **Three user roles**:
  - **Customer** – booking services and viewing appointment history
  - **Employee** – managing bookings and providing services
  - **Administrator** – full management of users, services, employees, and generating statistics

- **Service booking** via the calendar.
- **Booking confirmation** by employees or administrators.
- **Service and employee management** by the administrator.
