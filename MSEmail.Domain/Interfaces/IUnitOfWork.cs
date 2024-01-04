namespace MSEmail.Domain.Interfaces;

public interface IUnitOfWork
{
    public void Commit();
    public void Rollback();
}