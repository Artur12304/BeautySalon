import React, { useState, useEffect } from 'react';
import axios from '../services/api';

interface ServiceFormProps {
  serviceId?: string;
  onServiceCreated: () => void;
}

const ServiceForm: React.FC<ServiceFormProps> = ({
  serviceId,
  onServiceCreated,
}) => {
  const [name, setName] = useState('');
  const [price, setPrice] = useState<number>(0);
  const [duration, setDuration] = useState<number>(0);
  const [description, setDescription] = useState('');

  useEffect(() => {
    if (serviceId) {
      fetchServiceDetails(serviceId);
    }
  }, [serviceId]);

  const fetchServiceDetails = async (id: string) => {
    try {
      const response = await axios.get(`/api/services/${id}`);
      const service = response.data;
      setName(service.name);
      setPrice(service.price);
      setDuration(service.duration);
      setDescription(service.description);
    } catch (error) {
      console.error('Failed to fetch service details', error);
    }
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      if (serviceId) {
        await axios.put(`/api/services/${serviceId}`, {
          name,
          price,
          duration,
          description,
        });
      } else {
        await axios.post('/api/services', {
          name,
          price,
          duration,
          description,
        });
      }
      onServiceCreated();
    } catch (error) {
      console.error('Failed to create/update service', error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Service Name</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Price</label>
        <input
          type="number"
          value={price}
          onChange={(e) => setPrice(Number(e.target.value))}
          required
        />
      </div>
      <div>
        <label>Duration (minutes)</label>
        <input
          type="number"
          value={duration}
          onChange={(e) => setDuration(Number(e.target.value))}
          required
        />
      </div>
      <div>
        <label>Description</label>
        <textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />
      </div>
      <button type="submit">Save Service</button>
    </form>
  );
};

export default ServiceForm;
