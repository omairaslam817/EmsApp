using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ems.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        void BeginTransaction();
        void Commit();
        void Rollback();
    }

   
}
