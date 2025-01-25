namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using System.Net.Mail;

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<EmailService> _logger;

        public EmailService(SmtpClient smtpClient, ILogger<EmailService> logger)
        {
            _smtpClient = smtpClient;
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(string customerEmail, string emailBody)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email"),
                    Subject = "Booking Confirmation",
                    Body = emailBody,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(customerEmail.ToLower());

                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Confirmation email sent to {Email}", customerEmail);
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "Error sending email to {Email}", customerEmail);
                throw new Exception("An error occurred while sending the email. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email to {Email}", customerEmail);
                throw new Exception("An unexpected error occurred while sending the email.");
            }
        }
    }
}
