using VDCD.BL.Interface;
using Microsoft.EntityFrameworkCore;
using VDCD.BL.Services;
using VDCD.DL;
using VDCD.DL.Base;
using VDCD.DL.Interface;
using VDCD.DL.Repo;
using VDCD.DL.ConnectDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("VDCD.DL"))); // Migrations are in VDCD.DL project

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Cá»­a HÃ ng Gá»‘m Sá»© VDCD API",
        Version = "v1",
        Description = "Há»‡ thá»‘ng API quáº£n lÃ½ bÃ¡n hÃ ng cho cá»­a hÃ ng gá»‘m sá»©"
    });
});

// Register Repositories/DLs
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped<IProductDL, ProductDL>();
builder.Services.AddScoped<IOrderDL, OrderDL>();
builder.Services.AddScoped<IUserDL, UserDL>();

// Register Services
builder.Services.AddScoped(typeof(VDCD.BL.Interface.IBaseBL<>), typeof(VDCD.BL.BaseBL.BaseBL<>));
builder.Services.AddScoped<IBLProduct, BLProduct>();
builder.Services.AddScoped<IBLOrder, BLOrder>();
builder.Services.AddScoped<IBLStatistics, BLStatistics>();
builder.Services.AddScoped<IBLUser, BLUser>();

var app = builder.Build();

// Tự động Apply Migrations (tạo bảng) khi chạy dự án
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DBContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cá»­a HÃ ng Gá»‘m Sá»© VDCD API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
