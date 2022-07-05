using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habr.DataAccess.Data
{
    public interface IDataContext
    {
        DbSet<Post> Posts { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Draft> Drafts { get; set; }

        Task BeginTransactionAsync();

        Task CommitAsync(CancellationToken cancellationToken);

        Task Rollback();

        Task SaveAsync();

        Task SaveAsync(CancellationToken cancellationToken);
    }
}
