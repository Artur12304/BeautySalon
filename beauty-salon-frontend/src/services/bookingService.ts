import axios from 'axios';

export const getEmployeeBookings = async (
  startDate?: string,
  endDate?: string,
  status?: string,
) => {
  const params: any = {};
  if (startDate) params.startDate = startDate;
  if (endDate) params.endDate = endDate;
  if (status) params.status = status;

  return await axios.get('/api/bookings/employee', { params });
};
