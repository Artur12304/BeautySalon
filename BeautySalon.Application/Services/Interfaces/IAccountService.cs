namespace BeautySalon.Application.Services.Interfaces
{
    using BeautySalon.Domain.Models;
    using BeautySalon.Domain.Models.Requests;
    using System.Security.Claims;

    public interface IAccountService
    {
        Task<(bool Success, string? Token, string? Message)> LoginAsync(string email, string password);
        Task<(bool Success, string? Message)> RegisterCustomerAsync(CustomerRegisterRequest request);
        Task<(bool Success, string? Message)> RegisterEmployeeAsync(CreateEmployeeRequest request, Guid adminId);
        CurrentUser? GetCurrentUser(ClaimsPrincipal user);
    }
}
