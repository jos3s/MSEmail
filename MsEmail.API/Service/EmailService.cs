using Microsoft.Extensions.Options;
using MsEmail.API.Context;
using MsEmail.API.Entities;
using System.Net;
using System.Net.Mail;

namespace MsEmail.API.Service
{
    public class EmailService
    {
        private readonly IOptions<SmtpConfiguration> _options;
        private readonly SmtpClient _smtpClient;

        public EmailService(IOptions<SmtpConfiguration> options) {
            _options = options;
            _smtpClient = new SmtpClient
            {
                Host = _options.Value.Host,
                Port = _options.Value.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_options.Value.UserName, _options.Value.Password)
            };

        }

        public void SendEmail(Email email)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(email.EmailFrom);
                mail.To.Add(new MailAddress(email.EmailTo));
                mail.Subject = email.Subject;
                mail.Body = email.Body;
                email.Status = Entities.Enums.EmailStatus.Sent;
                _smtpClient.Send(mail);
            }
            catch (Exception)
            {
                email.Status = Entities.Enums.EmailStatus.Error;
            }
        }
    }
}
