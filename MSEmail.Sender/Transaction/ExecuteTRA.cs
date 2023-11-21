using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MsEmail.Sender.Service;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MSEmail.Infra.Repository;

namespace MSEmail.Sender.Transaction;

public class ExecuteTRA
{
    private readonly EmailRepository _emailRepository;
    private readonly CommonLog _commonLog;
    private readonly AppDbContext _context;

    public ExecuteTRA(AppDbContext context)
    {
        _context = context;
        _emailRepository = new EmailRepository(context);
        _commonLog = new CommonLog(context);
    }

    public void Execute()
    {
        try
        {
            List<Email> emails = _emailRepository.GetEmailsByStatus(EmailStatus.Created);

            emails = emails.Where(x => x.SendDate < DateTime.Now).ToList();

            foreach (Email item in emails)
            {
                Send(item);
            }
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Execute), this.GetType().Name, ServiceType.Microservice);
        }
    }

    private void Send(Email email)
    {
        try
        {
            EmailService.Send(email);

            _emailRepository.Save();
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Send), this.GetType().Name, ServiceType.Microservice);
        }
    }
}