using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Data
{
    public interface IDataContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectTask> Tasks { get; set; }   // j'utilise ProjectTask pour éviter conflit avec System.Threading.Tasks.Task
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Document> Documents { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
