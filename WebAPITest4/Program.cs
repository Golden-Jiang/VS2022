//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Utility.cs
// Description   : 系統所有公共函式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/27 09:30 建立於 D:\Golden\Project\VS2022\WebAPI_Test_3 目錄 
// Update Record :
// Note          : https://localhost:7079/openapi/v1.json
//                 https://localhost:7079/scalar/v1
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Scalar.AspNetCore;
using System.Net;
//
// iit SDK 
//
//using iitSystemWeb;
//using iitLogWeb;
//using iitDataWeb;
//using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest4
{
    public class Program
    {
        public static void Main( string [] args )
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if( app.Environment.IsDevelopment() )
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
