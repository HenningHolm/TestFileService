using FileStorageService.Models.Azure;
using FileStorageService.Models.Operations;
using FileStorageService.Models.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorageService.DI
{
    public static class Modules
    {
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAzureBlobService, AzureBlobService>();
            services.AddTransient<IGetFileDataByIdQuery, GetFileDataByIdQuery>();
            services.AddTransient<IGetFileListByPersonIdQuery, GetFileListByPersonIdQuery>();
            services.AddTransient<IUploadFileOperation, UploadFileOperation>();
            services.AddTransient<IDeleteFileOperation, DeleteFileOperation>();
            services.AddTransient<IDownloadFileOperation, DownloadFileOperation>();
            services.AddTransient<IGenerateSharedAccessUriOperation, GenerateSharedAccessUriOperation>();

        }
    }
}
