using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;
using AZR_RED_THREAD_DAL.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; } // DbSet for User entity
        DbSet<Roles> Roles { get; set; } // DbSet for Roles entity
        DbSet<UserTask> UserTasks { get; set; } // DbSet for UserTask entity
        DbSet<RolePrivilege> RolePrivileges { get; set; } // DbSet for RolePrivilege entity
        DbSet<Privilege> Privileges { get; set; } // DbSet for Privilege entity
        DbSet<Task.Task> Tasks { get; set; } // DbSet for Task entity
        DbSet<Document.Document> Documents { get; set; } // DbSet for Document entity
        DbSet<Project.Project> Projects { get; set; } // DbSet for Project entity
        DbSet<State.State> States { get; set; } // DbSet for State entity

        DatabaseFacade Database { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<IDbContextTransaction> BeginTransactionAsync();

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
