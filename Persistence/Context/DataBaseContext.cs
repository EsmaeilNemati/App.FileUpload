using Application.Interfaces.Contexts;
using Domain.Attributes;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DbSet<AttachmentType> AttachmentTypes { get ; set ; }
        public DbSet<Customer> Customers { get ; set ; }
        public DbSet<FileUpload> FileUploads { get ; set ; }

        public DataBaseContext(DbContextOptions option) : base(option)
        {

        }
      
 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AttachmentType>().HasData(
                 new AttachmentType { Id = 1, Name = "PDF",Extension=".pdf" },
                 new AttachmentType { Id = 2, Name = "Image",Extension = ".jpg,.jpeg,.png" }
             );
            builder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "مشتری نمونه 1" },
                new Customer { Id = 2, Name = "مشتری نمونه 2" }
            );


            //use Reflection
            foreach (var entitytype in builder.Model.GetEntityTypes())
            {
                if (entitytype.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    builder.Entity(entitytype.Name).Property<DateTime?>("InsertDate");
                    builder.Entity(entitytype.Name).Property<DateTime?>("UpdateDate");
                    builder.Entity(entitytype.Name).Property<DateTime?>("RemoveDate");
                    builder.Entity(entitytype.Name).Property<bool>("IsRemove").HasDefaultValue(false); ;
                }

            }
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified || p.State == EntityState.Added || p.State == EntityState.Deleted);
            foreach (var item in modifiedEntities)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                var inserted = entityType.FindProperty("InsertDate");
                var updated = entityType.FindProperty("UpdateDate");
                var deleted = entityType.FindProperty("RemoveDate");
                var IsRemoveed = entityType.FindProperty("IsRemove");
                if (item.State == EntityState.Added && inserted != null)
                {
                    item.Property("InsertDate").CurrentValue = DateTime.Now;
                }
                if (item.State == EntityState.Modified && updated != null)
                {
                    item.Property("UpdateDate").CurrentValue = DateTime.Now;
                }
                if (item.State == EntityState.Deleted && deleted != null && IsRemoveed != null)
                {
                    item.Property("RemoveDate").CurrentValue = DateTime.Now;
                    item.Property("IsRemove").CurrentValue = true;
                }
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified || p.State == EntityState.Added || p.State == EntityState.Deleted);
            foreach (var item in modifiedEntities)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                var inserted = entityType.FindProperty("InsertDate");
                var updated = entityType.FindProperty("UpdateDate");
                var deleted = entityType.FindProperty("RemoveDate");
                var IsRemoveed = entityType.FindProperty("IsRemove");
                if (item.State == EntityState.Added && inserted != null)
                {
                    item.Property("InsertDate").CurrentValue = DateTime.Now;
                }
                if (item.State == EntityState.Modified && updated != null)
                {
                    item.Property("UpdateDate").CurrentValue = DateTime.Now;
                }
                if (item.State == EntityState.Deleted && deleted != null && IsRemoveed != null)
                {
                    item.Property("RemoveDate").CurrentValue = DateTime.Now;
                    item.Property("IsRemove").CurrentValue = true;
                }
            }
            //return base.SaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
