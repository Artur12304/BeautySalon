namespace BeautySalon.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string customerEmail, string emailBody);
    }
}
