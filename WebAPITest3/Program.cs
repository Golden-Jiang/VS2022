using Microsoft.EntityFrameworkCore;
using WebAPI_Test_3.Models;
using WebAPITest3.Service;

namespace WebAPI_Test_3
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

            // 依賴注入 Logger
            //builder.Services.AddScoped<ILog1, Logger>();
            builder.Services.AddScoped<ILog1>( sp =>
            {
	            var msg = "Startup";
	            return new Logger(msg);            
            });
            
            // 定義 IConfiguration
            IConfiguration configuration = builder.Configuration;

            // iitSDKWeb 系統啟動
            Utility.Start(configuration, null);
           
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
