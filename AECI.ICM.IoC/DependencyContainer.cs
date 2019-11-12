using AECI.ICM.Application.Services;
using AECI.ICM.Data.Context;
using AECI.ICM.Data.Repository;
using AECI.ICM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AECI.ICM.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, 
                                                 IConfiguration configuration)
        {
            services.AddScoped<IICMService, ICMService>();
            services.AddScoped<IICMRepository, ICMRepository>();

            services.AddDbContext<ICMDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DBCtx"));
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Cors", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .Build();
                });
            });
        }
    }
}
