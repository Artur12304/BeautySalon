import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import { useNavigate } from 'react-router-dom';
import AdminSideMenu from './AdminSideMenu';
import '../styles/ManageEmployees.css';

interface Employee {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  specialization: string;
}

const ManageEmployees: React.FC = () => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [newEmployee, setNewEmployee] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    specialization: '',
  });
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [isDeleteConfirmationVisible, setIsDeleteConfirmationVisible] =
    useState(false);
  const [employeeToDelete, setEmployeeToDelete] = useState<Employee | null>(
    null,
  );
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    fetchEmployees();
  }, []);

  const fetchEmployees = async () => {
    try {
      const response = await axios.get('/api/employees');
      setEmployees(response.data);
    } catch (error) {
      setError('Failed to fetch employees.');
    }
  };

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    const { name, value } = e.target;
    setNewEmployee({
      ...newEmployee,
      [name]: value,
    });
  };

  const handleCreateEmployee = async () => {
    if (newEmployee.password !== newEmployee.confirmPassword) {
      setError('Passwords do not match!');
      return;
    }

    try {
      const response = await axios.post('/api/employees', newEmployee);
      console.log('Employee created:', response.data);
      setEmployees([response.data, ...employees]);
      setNewEmployee({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        confirmPassword: '',
        phoneNumber: '',
        specialization: '',
      });
      setIsFormVisible(false);
      navigate(0);
    } catch (error) {
      console.error('Failed to create employee', error);
      setError('Failed to create employee.');
    }
  };

  const handleDeleteClick = (employee: Employee) => {
    setEmployeeToDelete(employee);
    setIsDeleteConfirmationVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (employeeToDelete) {
      try {
        await axios.delete(`/api/employees/${employeeToDelete.id}`);
        setEmployees(employees.filter((e) => e.id !== employeeToDelete.id));
      } catch (error) {
        console.error('Failed to delete employee', error);
        setError('Failed to delete employee.');
      }
    }
    setIsDeleteConfirmationVisible(false);
    setEmployeeToDelete(null);
  };

  const handleCancelDelete = () => {
    setIsDeleteConfirmationVisible(false);
    setEmployeeToDelete(null);
  };

  return (
    <div className="manage-employees">
      <AdminSideMenu />
      <div className="manage-employees-content">
        <h2>Manage Employees</h2>
        {error && <p className="error-message">{error}</p>}
        <button
          onClick={() => setIsFormVisible(true)}
          className="add-employee-btn"
        >
          +
        </button>
        {isFormVisible && (
          <div className="employee-form">
            <h3>Create New Employee</h3>
            <label>First Name</label>
            <input
              type="text"
              name="firstName"
              value={newEmployee.firstName}
              onChange={handleInputChange}
            />
            <label>Last Name</label>
            <input
              type="text"
              name="lastName"
              value={newEmployee.lastName}
              onChange={handleInputChange}
            />
            <label>Email</label>
            <input
              type="email"
              name="email"
              value={newEmployee.email}
              onChange={handleInputChange}
            />
            <label>Password</label>
            <input
              type="password"
              name="password"
              value={newEmployee.password}
              onChange={handleInputChange}
            />
            <label>Confirm Password</label>
            <input
              type="password"
              name="confirmPassword"
              value={newEmployee.confirmPassword}
              onChange={handleInputChange}
            />
            <label>Phone Number</label>
            <input
              type="text"
              name="phoneNumber"
              value={newEmployee.phoneNumber}
              onChange={handleInputChange}
            />
            <label>Specialization</label>
            <input
              type="text"
              name="specialization"
              value={newEmployee.specialization}
              onChange={handleInputChange}
            />
            <button onClick={handleCreateEmployee}>Create Employee</button>
            <button onClick={() => setIsFormVisible(false)}>Cancel</button>
          </div>
        )}
        {isDeleteConfirmationVisible && (
          <div className="delete-confirmation">
            <p>
              Are you sure you want to delete the employee:{' '}
              <strong>
                {employeeToDelete?.firstName} {employeeToDelete?.lastName}
              </strong>
              ?
            </p>
            <button onClick={handleConfirmDelete}>Yes</button>
            <button onClick={handleCancelDelete}>No</button>
          </div>
        )}
        <div className="employee-list">
          {employees.map((employee) => (
            <div key={employee.id} className="employee-item">
              <div className="employee-details">
                <h3>
                  {employee.firstName} {employee.lastName}
                </h3>
                <p>
                  Email: <strong>{employee.email}</strong>
                </p>
                <p>
                  Phone: <strong>{employee.phoneNumber}</strong>
                </p>
                <p>
                  Specialization: <strong>{employee.specialization}</strong>
                </p>
              </div>
              <div className="employee-actions">
                <button onClick={() => handleDeleteClick(employee)}>
                  Delete
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ManageEmployees;
