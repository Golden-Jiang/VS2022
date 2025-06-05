//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Program.cs
// Description   : 系統啟動
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 11:00 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using iitToolsWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // IConfiguration
            IConfiguration configuration = builder.Configuration;

            // 使用封裝好的服務註冊類別
            ServiceRegistration.AddAllServices( builder.Services, configuration );

            // iitSDKWeb Service Start 
            iitSystemTools.SystemStart( configuration );

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        } // end of public static void Main(string[] args)
    } // end of public class Program
} // end of namespace WebAPITest6
//===================================================================================================
// end of Program.cs
//===================================================================================================
