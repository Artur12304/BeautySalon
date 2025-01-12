import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import EmployeeSideMenu from '../components/EmployeeSideMenu';
import '../styles/Services.css';

const Services: React.FC = () => {
  const [services, setServices] = useState<any[]>([]);
  const [newService, setNewService] = useState({
    id: '',
    name: '',
    description: '',
    defaultPrice: 0,
    duration: 0,
  });
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [selectedServiceId, setSelectedServiceId] = useState<string | null>(
    null,
  );
  const [isDeleteConfirmationVisible, setIsDeleteConfirmationVisible] =
    useState(false);
  const [serviceToDelete, setServiceToDelete] = useState<any>(null);

  useEffect(() => {
    fetchServices();
  }, []);

  const fetchServices = async () => {
    try {
      const response = await axios.get('/api/services');
      setServices(response.data);
    } catch (error) {
      console.error('Failed to fetch services', error);
    }
  };

  const handleAddService = async () => {
    try {
      const { id, ...serviceWithoutId } = newService;
      console.log('Sending new service:', serviceWithoutId);
      const response = await axios.post('/api/services', serviceWithoutId);
      console.log('Service added:', response.data);
      setServices([response.data, ...services]);
      resetForm();
      setIsFormVisible(false);
    } catch (error) {
      console.error('Failed to add service', error);
    }
  };

  const handleEditService = async () => {
    try {
      const response = await axios.put(
        `/api/services/${newService.id}`,
        newService,
      );
      const updatedServices = services.map((service) =>
        service.id === newService.id ? response.data : service,
      );
      setServices(updatedServices);
      resetForm();
      setIsFormVisible(false);
      setIsEditMode(false);
      setSelectedServiceId(null);
    } catch (error) {
      console.error('Failed to edit service', error);
    }
  };

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    const { name, value } = e.target;
    setNewService({
      ...newService,
      [name]: value,
    });
  };

  const resetForm = () => {
    setNewService({
      id: '',
      name: '',
      description: '',
      defaultPrice: 0,
      duration: 0,
    });
  };

  const handleEditClick = (service: any) => {
    setNewService(service);
    setIsFormVisible(false);
    setIsEditMode(true);
    setSelectedServiceId(service.id);
  };

  const handleResize = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    e.target.style.height = 'auto';
    e.target.style.height = `${e.target.scrollHeight}px`;
  };

  const handleDeleteClick = (service: any) => {
    setServiceToDelete(service);
    setIsDeleteConfirmationVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (serviceToDelete) {
      try {
        await axios.delete(`/api/services/${serviceToDelete.id}`);
        setServices(
          services.filter((service) => service.id !== serviceToDelete.id),
        );
      } catch (error) {
        console.error('Failed to delete service', error);
      }
    }
    setIsDeleteConfirmationVisible(false);
    setServiceToDelete(null);
  };

  const handleCancelDelete = () => {
    setIsDeleteConfirmationVisible(false);
    setServiceToDelete(null);
  };

  return (
    <div className="services-container">
      <EmployeeSideMenu />
      <div className="services-content">
        <h2>Services</h2>

        <button
          className="add-service-button"
          onClick={() => {
            setIsFormVisible(true);
            setIsEditMode(false);
            resetForm();
            setSelectedServiceId(null);
          }}
        >
          +
        </button>

        {isFormVisible && !isEditMode && (
          <div className="service-form add-form">
            <h3>Add New Service</h3>
            <label htmlFor="name">Service Name</label>
            <input
              type="text"
              name="name"
              id="name"
              placeholder="Service Name"
              value={newService.name}
              onChange={handleInputChange}
            />
            <label htmlFor="description">Description</label>
            <textarea
              name="description"
              id="description"
              placeholder="Description"
              value={newService.description}
              onChange={handleInputChange}
              onInput={handleResize}
            />
            <label htmlFor="defaultPrice">Default Price</label>
            <input
              type="number"
              name="defaultPrice"
              id="defaultPrice"
              placeholder="Default Price"
              value={newService.defaultPrice}
              onChange={handleInputChange}
            />
            <label htmlFor="duration">Duration (minutes)</label>
            <input
              type="number"
              name="duration"
              id="duration"
              placeholder="Duration (minutes)"
              value={newService.duration}
              onChange={handleInputChange}
            />
            <button
              className="service-form-add-button"
              onClick={handleAddService}
            >
              Add Service
            </button>
            <button
              className="service-form-cancel-button"
              onClick={() => setIsFormVisible(false)}
            >
              Cancel
            </button>
          </div>
        )}

        <div className="service-list">
          {services.map((service) => (
            <div
              key={service.id}
              className={`service-item ${
                selectedServiceId === service.id ? 'selected' : ''
              }`}
            >
              {isDeleteConfirmationVisible &&
              serviceToDelete?.id === service.id ? (
                <div className="delete-confirmation">
                  <p>
                    Are you sure you want to delete the service:{' '}
                    <strong>{serviceToDelete?.name}</strong>?
                  </p>
                  <button onClick={handleConfirmDelete}>Yes</button>
                  <button onClick={handleCancelDelete}>No</button>
                </div>
              ) : selectedServiceId === service.id ? (
                <div className="service-form edit-form">
                  <h3>Edit Service</h3>
                  <label htmlFor="name">Service Name</label>
                  <input
                    type="text"
                    name="name"
                    id="name"
                    placeholder="Service Name"
                    value={newService.name}
                    onChange={handleInputChange}
                  />
                  <label htmlFor="description">Description</label>
                  <textarea
                    name="description"
                    id="description"
                    placeholder="Description"
                    value={newService.description}
                    onChange={handleInputChange}
                  />
                  <label htmlFor="defaultPrice">Default Price</label>
                  <input
                    type="number"
                    name="defaultPrice"
                    id="defaultPrice"
                    placeholder="Default Price"
                    value={newService.defaultPrice}
                    onChange={handleInputChange}
                  />
                  <label htmlFor="duration">Duration (minutes)</label>
                  <input
                    type="number"
                    name="duration"
                    id="duration"
                    placeholder="Duration (minutes)"
                    value={newService.duration}
                    onChange={handleInputChange}
                  />
                  <button onClick={handleEditService}>Update Service</button>
                  <button onClick={() => setSelectedServiceId(null)}>
                    Cancel
                  </button>
                </div>
              ) : (
                <div>
                  <h3>{service.name}</h3>
                  <p>{service.description}</p>
                  <p>
                    <strong>Price:</strong> {service.defaultPrice} $
                  </p>
                  <p>
                    <strong>Duration:</strong> {service.duration} minutes
                  </p>
                  <button
                    className="edit-service-button"
                    onClick={() => handleEditClick(service)}
                  >
                    Edit
                  </button>
                  <button
                    className="delete-service-button"
                    onClick={() => handleDeleteClick(service)}
                  >
                    Delete
                  </button>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Services;
