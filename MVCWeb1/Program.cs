using iitToolsWeb;

namespace MVCWeb1
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
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // 支持靜態文件(圖片, 影音, *.html) middleware
            app.UseStaticFiles();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
