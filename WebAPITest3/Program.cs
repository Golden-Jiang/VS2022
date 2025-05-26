using Microsoft.EntityFrameworkCore;
using WebAPI_Test_3.Models;

namespace WebAPI_Test_3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ¨Ì¿àª`¤J
            builder.Services.AddDbContext<DBContext>(Options =>
                    Options.UseSqlServer(builder.Configuration.GetConnectionString("SMARTBANK")));

            // Add services to the container.
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
