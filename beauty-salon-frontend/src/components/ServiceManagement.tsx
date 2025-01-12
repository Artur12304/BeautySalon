import React, { useState, useEffect } from 'react';
import axios from '../services/api';

const ServiceManagement: React.FC = () => {
  const [services, setServices] = useState<any[]>([]);
  const [newService, setNewService] = useState<string>('');
  const [isAddingService, setIsAddingService] = useState<boolean>(false);

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
      const response = await axios.post('/api/services', { name: newService });
      setServices([...services, response.data]);
      setNewService('');
    } catch (error) {
      console.error('Failed to add service', error);
    }
  };

  return (
    <div>
      <h3>Manage Services</h3>
      <div>
        <h4>Services List</h4>
        <ul>
          {services.map((service) => (
            <li key={service.id}>{service.name}</li>
          ))}
        </ul>
      </div>
      <div>
        <h4>Add New Service</h4>
        {isAddingService ? (
          <>
            <input
              type="text"
              value={newService}
              onChange={(e) => setNewService(e.target.value)}
              placeholder="Service name"
            />
            <button onClick={handleAddService}>Add</button>
          </>
        ) : (
          <button onClick={() => setIsAddingService(true)}>
            Add New Service
          </button>
        )}
      </div>
    </div>
  );
};

export default ServiceManagement;
