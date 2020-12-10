using Microsoft.EntityFrameworkCore;
using StorageService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageService.Data
{
    public static class DbContextExtensions
    {
        public static FileStorageDbContext Read(this DbContextOptions<FileStorageDbContext> dbContextOptions)
        {
            var context = new FileStorageDbContext(dbContextOptions);
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }
        public static FileStorageDbContext Write(this DbContextOptions<FileStorageDbContext> dbContextOptions)
        {
            var context = new FileStorageDbContext(dbContextOptions);
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return context;
        }
    }
}
