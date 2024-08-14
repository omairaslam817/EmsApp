using Ems.Domain.Repositories;
using Ems.Persistence;

namespace Ems.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _transactionStarted;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error saving changes to the database.", ex);
        }
    }

    public void BeginTransaction()
    {
        if (!_transactionStarted)
        {
            _context.Database.BeginTransaction();
            _transactionStarted = true;
        }
    }

    public void Commit()
    {
        if (_transactionStarted)
        {
            _context.Database.CommitTransaction();
            _transactionStarted = false;
        }
    }

    public void Rollback()
    {
        if (_transactionStarted)
        {
            _context.Database.RollbackTransaction();
            _transactionStarted = false;
        }
    }
}