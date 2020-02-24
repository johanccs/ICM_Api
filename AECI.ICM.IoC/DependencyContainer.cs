using AECI.ICM.Application.Interfaces;
using AECI.ICM.Application.Models;
using AECI.ICM.Application.Services;
using AECI.ICM.Data.Context;
using AECI.ICM.Data.Repository;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Shared.Interfaces;
using AECI.ICM.Shared.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AECI.ICM.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, 
                                                 IConfiguration configuration)
        {
            services.AddScoped<IICMService, ICMService>();
            services.AddScoped<IICMRepository, ICMRepository>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<ISharedNotificationService, SharedEmailNotificationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<IBranchDirectoryService, BranchDirectoryService>();
            services.AddScoped<IBranch, Branch>();
            services.AddScoped<ICMFileService, ICMFileService>();
            services.AddScoped<Shared.Interfaces.ILogger, Shared.Service.EventLogger>();
            services.AddSingleton<IFileSystem, Application.Services.FileSystemService>();
          
            services.AddDbContext<ICMDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DBCtx"));
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            
            //services.AddCors();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Cors", builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //               .AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowCredentials()
            //               .Build();
            //    });
            //});
        }
    }
}
