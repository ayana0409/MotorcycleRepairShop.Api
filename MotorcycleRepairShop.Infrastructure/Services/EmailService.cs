using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Share.Configuration;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Motorcycle Repair Shop", _smtpSettings.Username));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.EnableSsl);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý
                throw new Exception("Failed to send email", ex);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
