using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Document> Documents { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Ici tu peux gérer la traçabilité (Traceability): set CreatedAt/UpdatedAt, user, soft-delete...
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exemple : nom de projet unique
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // config relations, noms de tables, conversion enum etc.
        }
    }
}
