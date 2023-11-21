using MsEmail.Infra.Context;
using MSEmail.Infra.Repository;

namespace MSEmail.Sender.Transaction;

public class ExecuteTRA
{
    private readonly EmailRepository _emailRepository;

    public ExecuteTRA(AppDbContext context)
    {
        _emailRepository = new EmailRepository(context);
    }

    public void Execute()
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }
}