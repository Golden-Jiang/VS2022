//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : ServicRegistration.cs
// Description   : 註冊系統所有服務
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 11:00 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using WebAPITest6.Models;
using iitLogWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices( this IServiceCollection services )
        {
            // 註冊應用程序服務
            services.AddScoped<IWebAPIRepository, WebAPIRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IiitLog, iitLog>();
        }

        public static void AddDatabaseServices( this IServiceCollection services, IConfiguration configuration )
        {
            // 註冊資料庫服務
            services.AddDbContext<DBContext>(Options =>
                    Options.UseSqlServer( configuration.GetConnectionString( "SMARTBANK" ) ) );

        }

        public static void AddSystemServices(this IServiceCollection services, IConfiguration configuration )
        {
            // IHttpContextAccessor
            services.AddHttpContextAccessor();
           
            services.AddControllersWithViews();

            services.AddControllers();
        }

        public static void AddAllServices(this IServiceCollection services, IConfiguration configuration )
        {
            AddApplicationServices( services );
            AddDatabaseServices( services, configuration );
            AddSystemServices( services, configuration );
            // 這裡可以添加更多的服務註冊方法
        }
    } // end of public static class ServiceRegistration
} // end of namespace WebAPITest6
//===================================================================================================
// end of ServicRegistration.cs
//===================================================================================================
