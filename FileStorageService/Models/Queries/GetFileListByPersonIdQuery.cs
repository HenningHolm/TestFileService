using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileStorageService.Data;
using FileStorageService.Models.RequestResponse;
using Microsoft.EntityFrameworkCore;
using StorageService.Data;

namespace FileStorageService.Models.Queries
{
    public interface IGetFileListByPersonIdQuery
    {
        Task<List<FileDataResponse>> ExecuteAsync(int personId);
    }

    public class GetFileListByPersonIdQuery : IGetFileListByPersonIdQuery
    {
        private readonly DbContextOptions<FileStorageDbContext> _dbContextOptions;

        public GetFileListByPersonIdQuery(DbContextOptions<FileStorageDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }
        public async Task<List<FileDataResponse>> ExecuteAsync(int personId)
        {
            using (var context = _dbContextOptions.Read())
            {
                return await context.Files.Where(o => o.UserId == personId)
                    .Select(m => new FileDataResponse()
                    {
                        Id = m.Id,
                        Filename = m.Filename,
                        Name = m.Name,
                        FileExtension = m.FileExtension,
                        FileSize = m.FileSize,
                        Description = m.Description,
                    })
                    .ToListAsync();
            }
        }
    }
}
