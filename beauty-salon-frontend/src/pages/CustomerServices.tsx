import React, { useState, useEffect } from 'react';
import axios from '../services/api';
import CustomerSideMenu from '../components/CustomerSideMenu';
import '../styles/Services.css';

const CustomerServices: React.FC = () => {
  const [services, setServices] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchServices();
  }, []);

  const fetchServices = async () => {
    try {
      const response = await axios.get('/api/services');
      setServices(response.data);
    } catch (error) {
      setError('Failed to fetch services.');
      console.error('Failed to fetch services', error);
    }
  };

  return (
    <div className="services-container">
      <CustomerSideMenu />
      <div className="services-content">
        <h2>Available Services</h2>

        {error && <p className="error-message">{error}</p>}

        {/* Lista usług */}
        <div className="service-list">
          {services.map((service) => (
            <div key={service.id} className="service-item">
              <h3>{service.name}</h3>
              <p>{service.description}</p>
              <p>
                <strong>Price:</strong> {service.defaultPrice} $
              </p>
              <p>
                <strong>Duration:</strong> {service.duration} minutes
              </p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default CustomerServices;
