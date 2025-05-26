using WebAPI_Test_2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Configuration.GetSection("Logging");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
