import React from "react";
import { useNavigate } from "react-router-dom";
import "../styles/ServiceSelection.css";

interface Service {
    id: string;
    name: string;
    description: string;
    price: number;
}

const ServiceSelection: React.FC<{ services: Service[] }> = ({ services }) => {
    const navigate = useNavigate();

    const handleServiceClick = (serviceId: string) => {
        navigate(`/calendar/${serviceId}`);
    };

    return (
        <div className="service-selection">
            <h2>Select a Service</h2>
            <div className="service-list">
                {services.map((service) => (
                    <div
                        key={service.id}
                        className="service-item"
                        onClick={() => handleServiceClick(service.id)}
                    >
                        <h3>{service.name}</h3>
                        <p>{service.description}</p>
                        <p>Price: ${service.price}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ServiceSelection;
