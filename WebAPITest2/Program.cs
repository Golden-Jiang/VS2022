//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Program.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/29 06:00 建立於 D:\Golden\Project\VS2022\WebAPITest2 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using WebAPITest2.Models;
//
// iitSDKWeb
//
//using iitSystemWeb;
//using iitLogWeb;
//using iitDataWeb;
//using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 依賴注入 DBContext
            builder.Services.AddDbContext<DBContext>(Options =>
                    Options.UseSqlServer(builder.Configuration.GetConnectionString("SMARTBANK")));

            // 註冊 IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // 定義 IConfiguration
            IConfiguration configuration = builder.Configuration;

            // iitSDKWeb 系統啟動
            Utility.Start(configuration);
           
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        } // end of public static void Main(string[] args)
    } // end of public class Program
} // end of namespace WebAPITest2
//===================================================================================================
// end of Program.cs
//===================================================================================================
