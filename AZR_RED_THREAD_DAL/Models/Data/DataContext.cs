using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;
using AZR_RED_THREAD_DAL.Models.State;
using AZR_RED_THREAD_DAL.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Data
{
    public class DataContext : DbContext, IDataContext
    {

        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DataContext() : base(new DbContextOptions<DataContext>()) { }

        public DbSet<User> Users { get; set; } // DbSet for User entity
        public DbSet<Roles> Roles { get; set; } // DbSet for Roles entity
        public DbSet<UserTask> UserTasks { get; set; } // DbSet for UserTask entity
        public DbSet<RolePrivilege> RolePrivileges { get; set; } // DbSet for RolePrivilege entity
        public DbSet<Privilege> Privileges { get; set; } // DbSet for Privilege entity
        public DbSet<Task.Task> Tasks { get; set; } // DbSet for Task entity
        public DbSet<Document.Document> Documents { get; set; } // DbSet for Document entity
        public DbSet<Project.Project> Projects { get; set; } // DbSet for Project entity
        public DbSet<State.State> States { get; set; } // DbSet for State entity


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Si _configuration est null (cas des migrations), utilise une chaîne de connexion par défaut ou lève une exception claire
                if (_configuration == null)
                {
                    // Pour les migrations, tu peux mettre une chaîne de connexion temporaire ici :
                    optionsBuilder.UseSqlServer(
                        "Server=localhost\\SQLEXPRESS;Database=AZR_RED_THREAD_DB;Trusted_Connection=True;TrustServerCertificate=True;",
                        builder => builder.MigrationsAssembly("AZR_RED_THREAD_DAL"));
                    return;
                }

                string environment = _configuration["Environment"];
                if (string.IsNullOrEmpty(environment))
                {
                    throw new InvalidOperationException("Environment configuration is missing.");
                }

                string connectionString = _configuration
                    .GetSection("ConnectionStrings")
                    .GetSection("DefaultConnection")
                    .GetSection(environment)
                    .Value;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        $"Connection string for environment '{environment}' not found.");
                }

                optionsBuilder.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly("AZR_RED_THREAD_DAL"));
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Database.BeginTransactionAsync();
        }

        public int SaveChanges() => base.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);
    }


}
