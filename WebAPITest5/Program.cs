using Microsoft.EntityFrameworkCore;
using iitToolsWeb;
using iitLogWeb;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI_Test_5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ���U IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // �w�q IConfiguration
            IConfiguration configuration = builder.Configuration;

            // 使用封裝好的服務註冊類別
            ServiceRegistration.AddAllServices( builder.Services, configuration );

            // iitSDKWeb 
            iitSystemTools.SystemStart( configuration );
           
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
