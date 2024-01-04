using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Common.Utils;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MSEmail.Infra.Repository;

namespace MSEmail.PrepareEmail.Transaction;

public class ExecuteTRA
{
    private EmailRepository _emails;
    private CommonLog _log;

    public ExecuteTRA(AppDbContext context)
    {
        _emails = new EmailRepository(context);
        _log = new CommonLog(context);
    }

    public void Execute(EmailStatus emailStatus)
    {
        try
        {
            List<Email> emails = _emails.GetEmailsByStatus(emailStatus);

            foreach (Email email in emails)
            {
                if (!email.SendDate.IsNull())
                    email.Status = EmailStatus.PreSend;
                else
                    email.Status = EmailStatus.Draft;

                email.UpdateUserId = ConfigHelper.DefaultUserId();
                _emails.UpdateStatus(email);
            }

            _emails.Save();
        }
        catch (Exception ex)
        {
            _log.SaveExceptionLog(ex, nameof(Execute), this.GetType().Name, ServiceType.MSPrepareEmail);
        }
    }
}