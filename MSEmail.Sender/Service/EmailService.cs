using MsEmail.Domain.Entities;
using MSEmail.Common.Utils;
using MSEmail.Domain.Enums;
using System.Net;
using System.Net.Mail;

namespace MsEmail.Sender.Service;

public static class EmailService
{
    private static SmtpClient _smtpClient => new SmtpClient
    {
        Host = ConfigHelper.GetSmtpHost,
        Port = ConfigHelper.GetSmtpPort,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(ConfigHelper.GetSmtpUserName, ConfigHelper.GetSmtpPassword)
    };

    public static void Send(Email email)
    {
        try
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(email.EmailFrom);
            mail.To.Add(new MailAddress(email.EmailTo));
            mail.Subject = email.Subject;
            mail.Body = email.Body;
            _smtpClient.Send(mail);
            email.Status = EmailStatus.Sent;
        }
        catch (Exception)
        {
            email.Status = EmailStatus.Error;
            throw;
        }
    }
}
