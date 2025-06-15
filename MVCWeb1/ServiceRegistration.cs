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

using MVCWeb1.Models;
using MVCWeb1.Interface;
using iitLogWeb;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace MVCWeb1
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices( this IServiceCollection services )
        {
            // 註冊應用程序服務
            services.AddTransient<IStudentRepository, MockStudentRepository>();

            //services.AddScoped<IReadOnlyDictionary<string, IRepository<T>>>( provider =>
            //{
            //    var allRepository = provider.GetService<IEnumerable<IRepository<T>>>();
            //    return allRepository.ToDictionary( p => p.Name, p => p );
            //});


            services.AddTransient<IiitLog, iitLog>();
        }

        public static void AddDatabaseServices( this IServiceCollection services, IConfiguration configuration )
        {
            // 註冊資料庫服務
            //services.AddDbContext<DBContext>(Options =>
            //        Options.UseSqlServer( configuration.GetConnectionString( "SMARTBANK" ) ) );

        }

        public static void AddSystemServices(this IServiceCollection services, IConfiguration configuration )
        {
            // IHttpContextAccessor
            services.AddHttpContextAccessor();
           
            // Add services to the container.
            services.AddMvc();
           
            // Add services to the container.
            services.AddRazorPages();

            services.AddControllersWithViews();

            services.AddControllers();
        }

        public static void AddSystemFilter(this IServiceCollection services, IConfiguration configuration )
        {
            //services.AddMvc( config => {
            //    config.Filters.Add( new AuthorizationFilter() );
            //    config.Filters.Add<ActionFilter>();              // 全局註冊過濾器
            //    config.Filters.Add( new ResultFilter() );
            //} );
        }

        public static void AddAllServices(this IServiceCollection services, IConfiguration configuration )
        {
            AddApplicationServices( services );
            AddDatabaseServices( services, configuration );
            AddSystemServices( services, configuration );
            AddSystemFilter( services, configuration );
            // 這裡可以添加更多的服務註冊方法
        }
    } // end of public static class ServiceRegistration
} // end of namespace WebAPITest6
//===================================================================================================
// end of ServicRegistration.cs
//===================================================================================================
