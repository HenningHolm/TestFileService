using FileStorageService.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Data
{
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options) : base(options) { }

        public DbSet<FileData> Files { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<FileData>(f =>
            {
                f.HasKey(e => e.Id);
                f.HasIndex(e => e.UserId);
                f.Property(e => e.UserId).HasMaxLength(50);
                f.Property(e => e.Filename).IsRequired().HasMaxLength(255);
                f.Property(e => e.Name).IsRequired().HasMaxLength(255);
                f.Property(e => e.FileExtension).IsRequired().HasMaxLength(20);
                f.Property(e => e.FileSize).IsRequired();
                f.Property(e => e.Description);
                f.Property(e => e.FullBlobName).IsRequired().HasMaxLength(255);
                f.Property(e => e.Category).IsRequired().HasMaxLength(50);
                f.Property(e => e.SourceApplication).IsRequired().HasMaxLength(50);
                f.Property(m => m.CreatedBy).HasMaxLength(255).IsRequired();
                f.Property(m => m.CreatedDate).IsRequired();
                f.Property(m => m.ModifiedBy).HasMaxLength(50);
                f.Property(m => m.ModifiedDate);
                f.Property(m => m.BlobUri).IsRequired();
                f.HasQueryFilter(m => EF.Property<bool>(m, nameof(m.IsDeleted)) == false);
                f.HasIndex(m => m.IsDeleted);
            });

            modelBuilder.Entity<AuditLog>(auditLog =>
            {
                auditLog.HasKey(a => a.Id);
                auditLog.Property(e => e.FileId).IsRequired();
                auditLog.Property(e => e.Action).HasMaxLength(255).IsRequired();
                auditLog.Property(e => e.ModifiedBy).HasMaxLength(255).IsRequired();
                auditLog.Property(e => e.Date).IsRequired();
                auditLog.Property(e => e.FullBlobName);
            });
        }
    }
}
