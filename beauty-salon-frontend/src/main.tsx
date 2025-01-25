import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import App from './App';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import LandingPage from './pages/LandingPage';
import DayView from './pages/DayView';
import BookingSummary from './pages/BookingSummary';
import Calendar from './pages/Calendar';
import Services from './pages/Services';
import AdminServices from './pages/AdminServices';
import Booking from './pages/Booking';
import './styles/global.css';
import CustomerBookingHistory from './components/CustomerBookingHistory';
import EmployeeBookingHistory from './components/EmployeeBookingHistory';
import EmployeeBookingManagement from './components/EmployeeBookingManagement';
import AdminBookingManagement from './components/AdminBookingManagement';
import ManageEmployees from './components/ManageEmployees';
import CustomerServices from './pages/CustomerServices';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        path: '/',
        element: <LandingPage />,
      },
      {
        path: 'login',
        element: <Login />,
      },
      {
        path: 'register',
        element: <Register />,
      },
      {
        path: 'dashboard',
        element: <Dashboard />,
      },
      {
        path: 'calendar/:date',
        element: <DayView />,
      },
      {
        path: 'booking-summary',
        element: <BookingSummary />,
      },
      {
        path: 'calendar',
        element: <Calendar />,
      },
      {
        path: 'services',
        element: <Services />,
      },
      {
        path: 'booking',
        element: <Booking />,
      },
      {
        path: 'employee-booking-history',
        element: <EmployeeBookingHistory />,
      },
      {
        path: 'customer-booking-history',
        element: <CustomerBookingHistory />,
      },
      {
        path: 'employee-booking-management',
        element: <EmployeeBookingManagement />,
      },
      {
        path: 'admin-services',
        element: <AdminServices />,
      },
      {
        path: 'admin-booking-management',
        element: <AdminBookingManagement />,
      },
      {
        path: 'manage-employees',
        element: <ManageEmployees />,
      },
      {
        path: 'customer-services',
        element: <CustomerServices />,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);
