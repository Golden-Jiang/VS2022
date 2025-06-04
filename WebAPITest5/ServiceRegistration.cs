using iitLogWeb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPITest5.Interface;
using WebAPITest5.Service;
using WebAPI_Test_5.Models;

namespace WebAPI_Test_5
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // 註冊應用程序服務
            services.AddScoped<IWebAPI.IBranch, WebAPIService.BranchService>();

            services.AddScoped<IiitLog, iitLog>();
        }

        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration )
        {
            // 註冊資料庫服務
            services.AddDbContext<DBContext>(Options =>
                    Options.UseSqlServer( configuration.GetConnectionString("SMARTBANK")));

        }

        public static void AddAllServices(this IServiceCollection services, IConfiguration configuration )
        {
            AddApplicationServices(services);
            AddDatabaseServices(services, configuration );
            // 這裡可以添加更多的服務註冊方法
        }
    }
}