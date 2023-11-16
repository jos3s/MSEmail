﻿using Microsoft.Extensions.Options;
using MS.Domain.Enums;
using MsEmail.Domain.Entities;
using MSEmail.Common.Utils;
using System.Net;
using System.Net.Mail;

namespace MsEmail.API.Service
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService() {
            _smtpClient = new SmtpClient
            {
                Host = ConfigHelper.GetSmtpHost(),
                Port = ConfigHelper.GetSmtpPort(),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigHelper.GetSmtpUserName(), ConfigHelper.GetSmtpPassword())
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
                email.Status = EmailStatus.Sent;
                _smtpClient.Send(mail);
            }
            catch (Exception)
            {
                email.Status = EmailStatus.Error;
            }
        }
    }
}
