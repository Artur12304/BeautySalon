namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Requests;
    using BeautySalon.Infrastructure.Repositories;
    using Microsoft.Extensions.Logging;

    public class ServiceService : IServiceService
    {
        private readonly IRepository<ServiceEntity> _serviceRepository;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(IRepository<ServiceEntity> serviceRepository, ILogger<ServiceService> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        public async Task<int> GetServiceCountAsync()
        {
            _logger.LogInformation("Fetching employee count");

            var count = await _serviceRepository.CountAsync();

            _logger.LogInformation("Employee count fetched: {Count}", count);

            return count;
        }

        public async Task<IEnumerable<ServiceEntity>> GetServicesAsync()
        {
            _logger.LogInformation("Fetching all services");
            return await _serviceRepository.GetAllAsync();
        }

        public async Task<ServiceEntity> GetServiceByIdAsync(Guid serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
            {
                _logger.LogWarning("Service with id {Id} not found", serviceId);
                return null;
            }
            return service;
        }

        public async Task<ServiceEntity> CreateServiceAsync(CreateServiceRequest serviceRequest)
        {
            _logger.LogInformation("Creating a new service with Name: {Name}", serviceRequest.Name);

            var service = new ServiceEntity
            {
                Name = serviceRequest.Name,
                Description = serviceRequest.Description,
                DefaultPrice = serviceRequest.DefaultPrice,
                Duration = serviceRequest.Duration,
            };

            await _serviceRepository.AddAsync(service);
            _logger.LogInformation("Service created successfully with Id: {Id}", service.Id);
            return service;
        }

        public async Task<ServiceEntity> UpdateServiceAsync(Guid id, ServiceEntity updatedService)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                _logger.LogWarning("Service with id {Id} not found", id);
                return null;
            }

            service.Name = updatedService.Name;
            service.Description = updatedService.Description;
            service.Price = updatedService.Price;
            service.Duration = updatedService.Duration;
            service.DefaultPrice = updatedService.DefaultPrice;

            await _serviceRepository.UpdateAsync(service);
            _logger.LogInformation("Service updated successfully with Id: {Id}", service.Id);
            return service;
        }

        public async Task<bool> DeleteServiceAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete service with Id: {Id}", id);

            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                _logger.LogWarning("Failed to delete service: Service not found for Id: {Id}", id);
                return false;
            }

            await _serviceRepository.DeleteAsync(id);
            _logger.LogInformation("Service deleted successfully with Id: {Id}", id);
            return true;
        }
    }
}