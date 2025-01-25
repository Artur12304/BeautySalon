namespace BeautySalon.Application.Services.Interfaces
{
    using System;

    public interface IJwtService
    {
        string GenerateToken(Guid userId, string role);
    }
}
