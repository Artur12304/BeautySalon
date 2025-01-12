namespace BeautySalon.Application.Services.Interfaces
{
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Dtos;
    using System;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        Task<int> GetEmployeeCountAsync();
        Task<EmployeeEntity> GetEmployeeByUserIdAsync(Guid userId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();
        Task<EmployeeEntity> GetAvailableEmployeeAsync(Guid serviceId, DateTime bookingDate);
        Task<EmployeeEntity> CreateEmployeeAsync(EmployeeEntity employee);
        Task<bool> DeleteEmployeeAsync(Guid id);
    }
}