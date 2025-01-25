namespace BeautySalon.Application.Services.Interfaces
{
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Requests;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IServiceService
    {
        Task<int> GetServiceCountAsync();
        Task<IEnumerable<ServiceEntity>> GetServicesAsync();
        Task<ServiceEntity> GetServiceByIdAsync(Guid serviceId);
        Task<ServiceEntity> CreateServiceAsync(CreateServiceRequest serviceRequest);
        Task<ServiceEntity> UpdateServiceAsync(Guid id, ServiceEntity updatedService);
        Task<bool> DeleteServiceAsync(Guid id);
    }
}
