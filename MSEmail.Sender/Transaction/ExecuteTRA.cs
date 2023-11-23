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
    private readonly UnitOfWork _uow;

    public ExecuteTRA(AppDbContext context)
    {
        _context = context;
        _emailRepository = new EmailRepository(context);
        _commonLog = new CommonLog(context);
        _uow = new UnitOfWork(context);
    }

    public void Execute()
    {
        try
        {
            List<Email> emails = _emailRepository.GetEmailsByStatus(EmailStatus.PreSend);

            emails = emails.Where(x => x.SendDate < DateTime.Now).ToList();

            foreach (Email item in emails)
            {
                Send(item);
            }
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Execute), this.GetType().Name, ServiceType.MSSender);
        }
    }

    private void Send(Email email)
    {
        try
        {
            EmailService.Send(email);

            _emailRepository.Update(email).Save();
        }
        catch (Exception ex)
        {
            _emailRepository.Update(email);
            _commonLog.SaveExceptionLog(ex, nameof(Send), this.GetType().Name, ServiceType.MSSender, uniqueTransaction: false);
            _uow.Commit();
        }
    }
}